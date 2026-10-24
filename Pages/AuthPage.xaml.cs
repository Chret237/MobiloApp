using MobiloApp.Controls;
using Microsoft.Extensions.DependencyInjection;
using MobiloApp.Services;

namespace MobiloApp.Pages;

/// <summary>Login and Register share the same layout (AuthScreen in the design); the subclasses pick the mode.</summary>
public partial class AuthPage : MobiloPage
{
	readonly bool _login;
	readonly AuthService _authService;

	public AuthPage(bool login)
	{
		_login = login;
		InitializeComponent();
		_authService = AppServices.Current.GetRequiredService<AuthService>();

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

	async void OnSubmit(object? sender, EventArgs e)
	{
		ErrorLabel.IsVisible = false;
		var email = EmailField.Text.Trim();
		var password = PasswordField.Text;

		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
		{
			ShowError("Enter your email and password to continue.");
			return;
		}

		if (!_login && password != ConfirmField.Text)
		{
			ShowError("The passwords do not match.");
			return;
		}

		try
		{
			if (_login)
				await _authService.SignInAsync(email, password);
			else
				await _authService.SignUpAsync(NameField.Text, email, PhoneField.Text, password);

			await Nav.Go("home");
		}
		catch (ApiUnavailableException)
		{
			ShowError("The server is unavailable. Check your connection and try again.");
		}
		catch (InvalidOperationException exception)
		{
			ShowError(exception.Message);
		}
	}

	void ShowError(string message)
	{
		ErrorLabel.Text = message;
		ErrorLabel.IsVisible = true;
	}
	async void OnSwitch(object? sender, TappedEventArgs e) => await Nav.Go(_login ? "register" : "login");
}

public class LoginPage : AuthPage { public LoginPage() : base(true) { } }
public class RegisterPage : AuthPage { public RegisterPage() : base(false) { } }
