await tp.Test("Status je 201 Created", async () =>
{
    Equal(201, (int)tp.Response.StatusCode);
});

await tp.Test("DivisionId uložený", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    NotNull((object)body.Id);
    tp.SetVariable("DivisionId", (int)body.Id);
});

await tp.Test("Údaje divízie sa zhodujú", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal("Divízia Technológií", (string)body.Name);
    Equal("DIV-TECH", (string)body.Code);
});
