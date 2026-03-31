#if !UNITY_WEBGL || UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 標準的なプラットフォーム（PC/Mobile/Console）用のエクスポート実装。
/// Task.Run を使用して別スレッドで処理を行い、メインスレッドのフリーズを回避する。
/// </summary>
public static class ExporterStandard
{
    public static void Export(
        LogItemCache streamCache, LogItemCache snapshotCache,
        IReadOnlyList<(int s0, int s1, int p0, int p1)> trials,
        string baseDirName, Action onComplete)
    {
        // 保存先パスの取得はメインスレッドで行う必要がある
        string savePathBase = Application.persistentDataPath;

        // 重い処理を別スレッドで実行
        Task.Run(() =>
        {
            try
            {
                ExportLogic(streamCache, snapshotCache, trials, baseDirName, savePathBase);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ExporterStandard] Error: {ex}");
            }
        }).ContinueWith(t =>
        {
            // 処理完了後、メインスレッドでコールバックを実行
            onComplete?.Invoke();
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    private static void ExportLogic(
        LogItemCache streamCache, LogItemCache snapshotCache,
        IReadOnlyList<(int s0, int s1, int p0, int p1)> trials,
        string baseDirName, string savePathBase)
    {
        var files = new List<LogExporter.FileInMemory>();
        var allSnapshotRows = new List<object[]>();

        // Snapshotヘッダ準備
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
                var (header, rows) = LogCsvGenerator.SliceCache(streamCache, s0, s1);
                if (rows.Count > 0)
                {
                    byte[] content = LogCsvGenerator.CreateCSVContent(header, rows);
                    string path = $"{baseDirName}/stream_trial{trialId}.csv";
                    files.Add(new LogExporter.FileInMemory { Path = path, Content = content });
                }
            }

            // Snapshotデータ蓄積
            if (p1 > p0)
            {
                LogCsvGenerator.AccumulateSnapshotRows(snapshotCache, p0, p1, trialId, allSnapshotRows);
            }
        }

        // 蓄積したSnapshotを一括出力
        if (allSnapshotRows.Count > 0)
        {
            var (finalHeader, finalRows) = LogCsvGenerator.FilterActiveColumns(rawSnapshotHeader, allSnapshotRows);
            byte[] content = LogCsvGenerator.CreateCSVContent(finalHeader, finalRows);
            string path = $"{baseDirName}/snapshot.csv";
            files.Add(new LogExporter.FileInMemory { Path = path, Content = content });
        }

        // ファイル書き出し（IO処理）
        foreach (var file in files)
        {
            string fullPath = Path.Combine(savePathBase, file.Path);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllBytes(fullPath, file.Content);
        }

        Debug.Log($"[ExporterStandard] Export complete: {Path.Combine(savePathBase, baseDirName)}");
    }
}
#endif