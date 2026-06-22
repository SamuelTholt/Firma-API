await tp.Test("Status je 204 No Content", async () =>
{
    Equal(204, (int)tp.Response.StatusCode);
});
