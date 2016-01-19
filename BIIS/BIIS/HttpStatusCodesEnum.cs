using System.ComponentModel;

namespace BIIS
{
    public enum HttpStatusCodesEnum
    {
        [Description("OK")]
        Ok = 200,
        [Description("Bad Request")]
        BadRequestError = 400,
        [Description("Not Found")]
        NotFoundError = 404,
        [Description("Internal Server Error")]
        InternalServerError = 500
    }
}