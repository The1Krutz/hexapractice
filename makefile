build:
	dotnet build
clean:
	rm -rf Monohexa/obj Monohexa/bin
	rm -rf Monohexa.Test/obj Monohexa.Test/bin Monohexa.Test/TestResults
	dotnet clean
restore:
	dotnet restore
run:
	dotnet run --project Monohexa
run-watch:
	dotnet watch --project Monohexa run
test:
	dotnet test --collect:"XPlat Code Coverage" --logger html && \
  make coverage-report
test-watch:
	dotnet watch --project Monohexa.Test test
content:
	(cd Monohexa && dotnet tool run mgcb-editor-linux Content/Content.mgcb)
coverage-report:
	(cd Monohexa.Test && dotnet reportgenerator)
open-coverage:
	firefox Monohexa.Test/TestResults/coverage/index.html