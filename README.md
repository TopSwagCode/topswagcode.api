# topswagcode.api

Todo:
* Add EF Core or Dapper to Postgres
* OpenTelemetry extra project to call, to see dependencies.
  * Add extra project to see dependencies
  * Add AWS exporter to see cloud service https://github.com/aws-observability/aws-otel-dotnet/blob/main/integration-test-app/docker-compose.yml
  * Add Prometheus to see counters
  * Add Grafana to see dashboards
* Unit and Integration Tests. (TestContainers??)

docker run --rm -d -p 9411:9411 --name zipkin openzipkin/zipkin
