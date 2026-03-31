#if UNITY_WEBGL && !UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

/// <summary>
/// WebGL用のエクスポート実装。
/// コルーチンを使用してメインスレッドで時分割処理を行い、ブラウザのフリーズを軽減する。
/// </summary>
public static class ExporterWebGL
{
    // jslibプラグイン側の関数定義
    [DllImport("__Internal")]
    private static extern void DownloadZip(string zipName, string paths, byte[] contents, int[] sizes, int count);

    public static void Export(
        MonoBehaviour runner,
        LogItemCache streamCache, LogItemCache snapshotCache,
        IReadOnlyList<(int s0, int s1, int p0, int p1)> trials,
        string baseDirName, Action onComplete)
    {
        runner.StartCoroutine(ExportRoutine(streamCache, snapshotCache, trials, baseDirName, onComplete));
    }

    private static IEnumerator ExportRoutine(
        LogItemCache streamCache, LogItemCache snapshotCache,
        IReadOnlyList<(int s0, int s1, int p0, int p1)> trials,
        string baseDirName, Action onComplete)
    {
        var files = new List<LogExporter.FileInMemory>();
        var allSnapshotRows = new List<object[]>();
        
        var rawSnapshotHeader = new List<string> { "trial_index" };
        if (snapshotCache.Header != null) { rawSnapshotHeader.AddRange(snapshotCache.Header); }

        // 試行ごとのループ
        for (int i = 0; i < trials.Count; i++)
        {
            var (s0, s1, p0, p1) = trials[i];
            int trialId = i + 1;

            // Streamデータ処理
            if (s1 > s0)
            {
                // リスト操作は同期的に行う（負荷軽減のためここは一括）
                var (header, rows) = LogCsvGenerator.SliceCache(streamCache, s0, s1);
                if (rows.Count > 0)
                {
                    // 最も重いCSV文字列生成のみコルーチンで分散処理
                    byte[] content = null;
                    yield return CreateCSVContentRoutine(header, rows, result => content = result);
                    
                    string path = $"{baseDirName}/stream_trial{trialId}.csv";
                    files.Add(new LogExporter.FileInMemory { Path = path, Content = content });
                }
            }

            // Snapshotデータ蓄積
            if (p1 > p0)
            {
                LogCsvGenerator.AccumulateSnapshotRows(snapshotCache, p0, p1, trialId, allSnapshotRows);
            }

            // 試行ごとに1フレーム休憩
            yield return null;
        }

        // Snapshot統合出力
        if (allSnapshotRows.Count > 0)
        {
            var (finalHeader, finalRows) = LogCsvGenerator.FilterActiveColumns(rawSnapshotHeader, allSnapshotRows);
            
            byte[] content = null;
            yield return CreateCSVContentRoutine(finalHeader, finalRows, result => content = result);

            string path = $"{baseDirName}/snapshot.csv";
            files.Add(new LogExporter.FileInMemory { Path = path, Content = content });
        }

        // JS呼び出しでZipダウンロード
        ProcessFiles(files, $"{baseDirName}.zip");
        
        onComplete?.Invoke();
    }

    /// <summary>
    /// LogCsvGenerator.CreateCSVContent のコルーチン版。
    /// 大量の行を処理する際、定期的に yield return null を挟んでフリーズを防ぐ。
    /// </summary>
    private static IEnumerator CreateCSVContentRoutine(List<string> header, List<object[]> rows, Action<byte[]> onResult)
    {
        var sb = new StringBuilder();

        // ヘッダ
        var headerCells = header.Select(h => CsvUtility.EscapeCSV(h)).ToList();
        sb.AppendLine(CsvUtility.JoinRow(headerCells));

        // データ
        int count = 0;
        var rowBuffer = new List<string>(header.Count);

        foreach (var row in rows)
        {
            rowBuffer.Clear();
            int rowLen = row.Length;
            for (int i = 0; i < rowLen && i < header.Count; i++)
            {
                rowBuffer.Add(CsvUtility.ToEscapedCell(row[i]));
            }
            for (int i = rowLen; i < header.Count; i++)
            {
                rowBuffer.Add(string.Empty);
            }
            sb.AppendLine(CsvUtility.JoinRow(rowBuffer));

            count++;
            // 500行ごとに1フレーム休憩（環境に合わせて調整可）
            if (count % 500 == 0)
            {
                yield return null;
            }
        }

        onResult?.Invoke(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray());
    }

    private static void ProcessFiles(List<LogExporter.FileInMemory> files, string zipFileName)
    {
        if (files.Count == 0) { return; }

        var paths = files.Select(f => f.Path.Replace(Path.DirectorySeparatorChar, '/')).ToList();
        var contents = new List<byte>();
        var sizes = new List<int>();

        foreach (var f in files)
        {
            contents.AddRange(f.Content);
            sizes.Add(f.Content.Length);
        }

        DownloadZip(zipFileName, string.Join("|", paths), contents.ToArray(), sizes.ToArray(), files.Count);
    }
}
#endif