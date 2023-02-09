# aspnetmvc-blog

dotnet run

dotnet publish -c Release -o ./publish

dotnet list package

dotnet add package

dotnet remove package

docker build -t allen456/aspnetmvc-blog:test1 .

docker run -p 5001:80 -d --restart unless-stopped allen456/aspnetmvc-blog:test1

docker push allen456/aspnetmvc-blog:test1
