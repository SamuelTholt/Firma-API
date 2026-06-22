await tp.Test("Status je 201 Created", async () =>
{
    Equal(201, (int)tp.Response.StatusCode);
});

await tp.Test("DepartmentId uložený", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    NotNull((object)body.Id);
    tp.SetVariable("DepartmentId", (int)body.Id);
});

await tp.Test("Údaje oddelenia sa zhodujú", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal("Oddelenie Vývoja", (string)body.Name);
    Equal("DEP-DEV", (string)body.Code);
});
