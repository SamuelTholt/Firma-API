await tp.Test("Status je 201 Created", async () =>
{
    Equal(201, (int)tp.Response.StatusCode);
});

await tp.Test("ProjectId uložený", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    NotNull((object)body.Id);
    tp.SetVariable("ProjectId", (int)body.Id);
});

await tp.Test("Údaje projektu sa zhodujú", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal("Projekt Alpha", (string)body.Name);
    Equal("PROJ-ALPHA", (string)body.Code);
});
