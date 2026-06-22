await tp.Test("Status je 200 OK", async () =>
{
    Equal(200, (int)tp.Response.StatusCode);
});

await tp.Test("Správny projekt vrátený", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal(tp.GetVariable<int>("ProjectId"), (int)body.Id);
    Equal("PROJ-ALPHA", (string)body.Code);
});
