start cmd /c docker build -t coinflipper-tracer -f ./Modules/CoinFlipper.Tracer/Dockerfile .
start cmd /c docker build -t coinflipper-notification -f ./Modules/CoinFlipper.Notification/Dockerfile .
start cmd /c docker build -t coinflipper-swissarmy -f ./Modules/CoinFlipper.SwissArmy/Dockerfile .
start cmd /c docker build -t coinflipper-access -f ./Modules/CoinFlipper.Access/Dockerfile .