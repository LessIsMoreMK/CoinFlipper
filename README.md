
docker build -t coinflipper-tracer -f ./Modules/CoinFlipper.Tracer/Dockerfile .
docker build -t coinflipper-notification -f ./Modules/CoinFlipper.Notification/Dockerfile .
docker build -t coinflipper-swissarmy -f ./Modules/CoinFlipper.SwissArmy/Dockerfile .
                --no-cache


docker-compose up 
                    -d

docker-compose down 
                    -v   