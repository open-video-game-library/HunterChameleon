# エラー時は停止
set -e

# ビルド出力ディレクトリに移動
cd Builds/HunterChameleonWebGL

git init
git checkout -B main
git add -A
git commit -m 'deploy'

# https://<USERNAME>.github.io/<REPO> にデプロイする場合
git push -f git@github.com:open-video-game-library/HunterChameleon.git main:gh-pages

cd -