# Telerik Reporting REST Service
This is Telerik Reporting REST Service project that we use for testing different use cases such as testing features and also reproducing different issues and share them.

**Docker** (Linux OS) also is enabled. So, you can run this project via Docker. Then you can test Telerik Reporting functionalities in Linux environment.

The project has test reports in Reports folder to test different cases. For example, there is **Orders report.trdx** report which uses web service datasource with **odata service - Northwind** data for testing OData realated stuff (performance, OData calls,...).

Integrated **OpenTelemetry** with the Reporting Service project to analyze the project’s performance with metrics, logs, and traces. To visualize them in **Jaeger** you need to download the Jaeger first, run "jaeger-all-in-one.exe" in command prompt and then go to Jaeger dashboard http://localhost:16686/.


