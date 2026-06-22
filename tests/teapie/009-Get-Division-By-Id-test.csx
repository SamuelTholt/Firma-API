await tp.Test("Status je 200 OK", async () =>
{
    Equal(200, (int)tp.Response.StatusCode);
});

await tp.Test("Správna divízia vrátená", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal(tp.GetVariable<int>("DivisionId"), (int)body.Id);
    Equal("DIV-TECH", (string)body.Code);
});
