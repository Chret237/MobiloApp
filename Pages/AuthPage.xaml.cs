using MobiloApp.Controls;

namespace MobiloApp.Pages;

/// <summary>Login and Register share the same layout (AuthScreen in the design); the subclasses pick the mode.</summary>
public partial class AuthPage : MobiloPage
{
	readonly bool _login;

	public AuthPage(bool login)
	{
		_login = login;
		InitializeComponent();

		EyebrowLabel.Text = login ? "Welcome back" : "Join MOBILO";
		TitleLabel.Text = login ? "Let’s get you moving." : "Create your account.";
		SubtitleLabel.Text = login
			? "Sign in to manage bookings and continue your journey."
			: "One account for simpler, safer car rental.";
		SubmitButton.Text = login ? "Log in" : "Create account";
		SwitchPrompt.Text = login ? "New to MOBILO?" : "Already have an account?";
		SwitchAction.Text = login ? "Create account" : "Log in";

		NameField.IsVisible = PhoneField.IsVisible = ConfirmField.IsVisible = TermsRow.IsVisible = !login;
		ForgotLabel.IsVisible = login;
	}

	async void OnSubmit(object? sender, EventArgs e) => await Nav.Go("home");
	async void OnSwitch(object? sender, TappedEventArgs e) => await Nav.Go(_login ? "register" : "login");
}

public class LoginPage : AuthPage { public LoginPage() : base(true) { } }
public class RegisterPage : AuthPage { public RegisterPage() : base(false) { } }
