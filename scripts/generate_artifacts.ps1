New-Item -ItemType Directory -Force -Path release
git archive -o release/source.zip HEAD . ":!docs"
git archive -o release/release.zip HEAD .env docker-compose.yaml start.ps1 start.sh
