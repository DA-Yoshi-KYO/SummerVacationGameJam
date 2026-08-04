@echo off
chcp 65001 > nul
echo ================================
echo   Git Hooks セットアップ
echo ================================
echo.

git config core.hooksPath .githooks

if %errorlevel% equ 0 (
    echo Git hooks path を .githooks に設定しました。
    echo コミットメッセージのprefixチェックが有効になります。
) else (
    echo エラー: 設定に失敗しました。Gitリポジトリ内で実行しているか確認してください。
)

echo.
pause
