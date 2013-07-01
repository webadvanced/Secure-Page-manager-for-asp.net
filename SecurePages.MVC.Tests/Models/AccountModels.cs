namespace HTTPSManager.MVC.Web.Tests.Models {
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;

    public class UsersContext : DbContext {
        #region Constructors and Destructors

        public UsersContext()
            : base("DefaultConnection") {
        }

        #endregion

        #region Public Properties

        public DbSet<UserProfile> UserProfiles { get; set; }

        #endregion
    }

    [Table("UserProfile")]
    public class UserProfile {
        #region Public Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string UserName { get; set; }

        #endregion
    }

    public class RegisterExternalLoginModel {
        #region Public Properties

        public string ExternalLoginData { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        #endregion
    }

    public class LocalPasswordModel {
        #region Public Properties

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        #endregion
    }

    public class LoginModel {
        #region Public Properties

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        #endregion
    }

    public class RegisterModel {
        #region Public Properties

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        #endregion
    }

    public class ExternalLogin {
        #region Public Properties

        public string Provider { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ProviderUserId { get; set; }

        #endregion
    }
}