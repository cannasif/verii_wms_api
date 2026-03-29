using Wms.Application.Common;

namespace Wms.Modules.Identity.Localization;

public sealed class IdentityLocalizationResource : ILocalizationResource
{
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> MessagesByCulture { get; } = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
    {
        ["en"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["OnlyUserAndAdminRolesAllowed"] = "Only User And Admin Roles Allowed",
            ["Success.User.LoginSuccessful"] = "Success User Login Successful",
            ["UserAlreadyExists"] = "User Already Exists",
            ["UserAuthorityCannotDeleteWhenUsersAssigned"] = "User Authority Cannot Delete When Users Assigned",
            ["UserAuthorityCreatedSuccessfully"] = "User Authority Created Successfully",
            ["UserAuthorityDeletedSuccessfully"] = "User Authority Deleted Successfully",
            ["UserAuthorityNotFound"] = "User Authority Not Found",
            ["UserAuthorityRetrievedSuccessfully"] = "User Authority Retrieved Successfully",
            ["UserAuthorityUpdatedSuccessfully"] = "User Authority Updated Successfully",
            ["UserCreatedSuccessfully"] = "User Created Successfully",
            ["UserDeletedSuccessfully"] = "User Deleted Successfully",
            ["UserDetailAlreadyExists"] = "User Detail Already Exists",
            ["UserDetailCreatedSuccessfully"] = "User Detail Created Successfully",
            ["UserDetailDeletedSuccessfully"] = "User Detail Deleted Successfully",
            ["UserDetailNotFound"] = "User Detail Not Found",
            ["UserDetailNotFoundCanCreate"] = "User Detail Not Found Can Create",
            ["UserDetailRetrievedSuccessfully"] = "User Detail Retrieved Successfully",
            ["UserDetailUpdatedSuccessfully"] = "User Detail Updated Successfully",
            ["ProfilePictureUploadedSuccessfully"] = "Profile picture uploaded successfully",
            ["UserRetrievedSuccessfully"] = "User Retrieved Successfully",
            ["UserUpdatedSuccessfully"] = "User Updated Successfully"
        },
        ["tr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["OnlyUserAndAdminRolesAllowed"] = "Sadece User ve Admin rolleri kullanilabilir",
            ["Success.User.LoginSuccessful"] = "Giris basariyla tamamlandi",
            ["UserAlreadyExists"] = "Kullanici zaten mevcut",
            ["UserAuthorityCannotDeleteWhenUsersAssigned"] = "Kullanicilara atanmis yetki grubu silinemez",
            ["UserAuthorityCreatedSuccessfully"] = "Kullanici yetkisi basariyla olusturuldu",
            ["UserAuthorityDeletedSuccessfully"] = "Kullanici yetkisi basariyla silindi",
            ["UserAuthorityNotFound"] = "Kullanici yetkisi bulunamadi",
            ["UserAuthorityRetrievedSuccessfully"] = "Kullanici yetkileri basariyla getirildi",
            ["UserAuthorityUpdatedSuccessfully"] = "Kullanici yetkisi basariyla guncellendi",
            ["UserCreatedSuccessfully"] = "Kullanici basariyla olusturuldu",
            ["UserDeletedSuccessfully"] = "Kullanici basariyla silindi",
            ["UserDetailAlreadyExists"] = "Kullanici detayi zaten mevcut",
            ["UserDetailCreatedSuccessfully"] = "Kullanici detayi basariyla olusturuldu",
            ["UserDetailDeletedSuccessfully"] = "Kullanici detayi basariyla silindi",
            ["UserDetailNotFound"] = "Kullanici detayi bulunamadi",
            ["UserDetailNotFoundCanCreate"] = "Kullanici detayi bulunamadi, yeni kayit olusturulabilir",
            ["UserDetailRetrievedSuccessfully"] = "Kullanici detaylari basariyla getirildi",
            ["UserDetailUpdatedSuccessfully"] = "Kullanici detayi basariyla guncellendi",
            ["ProfilePictureUploadedSuccessfully"] = "Profil resmi basariyla yuklendi",
            ["UserRetrievedSuccessfully"] = "Kullanicilar basariyla getirildi",
            ["UserUpdatedSuccessfully"] = "Kullanici basariyla guncellendi"
        },
        ["fr"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["OnlyUserAndAdminRolesAllowed"] = "Only User And Admin Roles Allowed",
            ["Success.User.LoginSuccessful"] = "Success User Login Successful",
            ["UserAlreadyExists"] = "User Already Exists",
            ["UserAuthorityCannotDeleteWhenUsersAssigned"] = "User Authority Cannot Delete When Users Assigned",
            ["UserAuthorityCreatedSuccessfully"] = "User Authority Created Successfully",
            ["UserAuthorityDeletedSuccessfully"] = "User Authority Deleted Successfully",
            ["UserAuthorityNotFound"] = "User Authority Not Found",
            ["UserAuthorityRetrievedSuccessfully"] = "User Authority Retrieved Successfully",
            ["UserAuthorityUpdatedSuccessfully"] = "User Authority Updated Successfully",
            ["UserCreatedSuccessfully"] = "User Created Successfully",
            ["UserDeletedSuccessfully"] = "User Deleted Successfully",
            ["UserDetailAlreadyExists"] = "User Detail Already Exists",
            ["UserDetailCreatedSuccessfully"] = "User Detail Created Successfully",
            ["UserDetailDeletedSuccessfully"] = "User Detail Deleted Successfully",
            ["UserDetailNotFound"] = "User Detail Not Found",
            ["UserDetailNotFoundCanCreate"] = "User Detail Not Found Can Create",
            ["UserDetailRetrievedSuccessfully"] = "User Detail Retrieved Successfully",
            ["UserDetailUpdatedSuccessfully"] = "User Detail Updated Successfully",
            ["ProfilePictureUploadedSuccessfully"] = "Profile picture uploaded successfully",
            ["UserRetrievedSuccessfully"] = "User Retrieved Successfully",
            ["UserUpdatedSuccessfully"] = "User Updated Successfully"
        }
    };
}
