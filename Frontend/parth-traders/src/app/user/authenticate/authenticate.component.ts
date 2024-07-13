import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-authenticate',
  templateUrl: './authenticate.component.html',
  styleUrl: './authenticate.component.css'
})
export class AuthenticateComponent implements OnInit {
  registering: boolean = false;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    const savedUsername = localStorage.getItem("rememberedEmail");

    if (savedUsername) {
      const userField = document.getElementById("user") as HTMLInputElement | null;
      if (userField) {
        userField.value = savedUsername;
      }

      const rememberCheckbox = document.getElementById("remember") as HTMLInputElement | null;
      if (rememberCheckbox) {
        rememberCheckbox.checked = true;
      }
    }
  }

  togglePasswordVisibility(fieldId: string, event: Event): void {
    const passwordField = document.getElementById(fieldId) as HTMLInputElement | null;
    if (passwordField) {
      const type = passwordField.getAttribute("type") === "password" ? "text" : "password";
      passwordField.setAttribute("type", type);
      const target = event.target as HTMLElement | null;
      if (target) {
        target.classList.toggle("bx-hide");
        target.classList.toggle("bx-show");
      }
    }
  }

  openModal(modalId: string, event: Event): void {
    event.preventDefault();
    const modal = document.getElementById(modalId) as HTMLElement | null;
    if (modal) {
      modal.style.display = "block";
    }
  }

  closeModal(modalId: string): void {
    const modal = document.getElementById(modalId) as HTMLElement | null;
    if (modal) {
      modal.style.display = "none";
    }
  }

  toggleAuthForms(event: Event): void {
    event.preventDefault();
    this.registering = !this.registering;
  }

  handleLogin(): void {
    this.clearErrors();

    const email = (document.getElementById("user") as HTMLInputElement | null)?.value;
    const password = (document.getElementById("pass") as HTMLInputElement | null)?.value;
    const rememberMe = (document.getElementById("remember") as HTMLInputElement | null)?.checked;

    let valid = true;

    if (!email) {
      this.showError((document.getElementById("user") as HTMLInputElement | null), "Email is required");
      valid = false;
    }

    if (!password) {
      this.showError((document.getElementById("pass") as HTMLInputElement | null), "Password is required");
      valid = false;
    }

    if (!valid) {
      return;
    }

    if (rememberMe) {
      localStorage.setItem("rememberedEmail", email!);
    } else {
      localStorage.removeItem("rememberedEmail");
    }

    // Perform login operation
    this.authService.loginApi(email!, password!).then(result => {
      if (result.success) {
        alert("Logged in successfully");
      } else {
        this.showError((document.getElementById("pass") as HTMLInputElement | null), result.message);
      }
    }).catch(error => {
      this.showError((document.getElementById("pass") as HTMLInputElement | null), "Login failed. Please try again.");
    });
  }

  handleRegister(): void {
    this.clearErrors();

    const username = (document.getElementById("registerUser") as HTMLInputElement | null)?.value;
    const email = (document.getElementById("registerEmail") as HTMLInputElement | null)?.value;
    const password = (document.getElementById("registerPass") as HTMLInputElement | null)?.value;

    let valid = true;

    if (!username) {
      this.showError((document.getElementById("registerUser") as HTMLInputElement | null), "Username is required");
      valid = false;
    }

    if (!email) {
      this.showError((document.getElementById("registerEmail") as HTMLInputElement | null), "Email is required");
      valid = false;
    }

    if (!password) {
      this.showError((document.getElementById("registerPass") as HTMLInputElement | null), "Password is required");
      valid = false;
    }

    if (!valid) {
      return;
    }

    // Perform register operation
    this.authService.registerApi(username!, email!, password!).then(result => {
      if (result.success) {
        alert("Registration successful");
      } else {
        this.showError((document.getElementById("registerPass") as HTMLInputElement | null), result.message);
      }
    }).catch(error => {
      this.showError((document.getElementById("registerPass") as HTMLInputElement | null), "Registration failed. Please try again.");
    });
  }

  handleForgotPassword(): void {
    this.clearErrors();

    const email = (document.getElementById("forgotEmail") as HTMLInputElement | null)?.value;

    if (!email) {
      this.showError((document.getElementById("forgotEmail") as HTMLInputElement | null), "Email is required");
      return;
    }

    // Perform forgot password operation
    this.authService.forgotPasswordApi(email!).then(result => {
      if (result.success) {
        alert("Password reset link sent to your email");
        this.closeModal("forgotPasswordModal");
      } else {
        this.showError((document.getElementById("forgotEmail") as HTMLInputElement | null), result.message);
      }
    }).catch(error => {
      this.showError((document.getElementById("forgotEmail") as HTMLInputElement | null), "Password reset failed. Please try again.");
    });
  }

  checkPasswordStrength(event: Event): void {
    const strengthIndicator = document.getElementById("passwordStrength") as HTMLElement | null;
    if (strengthIndicator) {
      this.updatePasswordStrength((event.target as HTMLInputElement).value, strengthIndicator);
    }
  }

  checkPasswordStrengthRegister(event: Event): void {
    const strengthIndicator = document.getElementById("passwordStrengthRegister") as HTMLElement | null;
    if (strengthIndicator) {
      this.updatePasswordStrength((event.target as HTMLInputElement).value, strengthIndicator);
    }
  }

  updatePasswordStrength(password: string, strengthIndicator: HTMLElement): void {
    // Reset indicator
    strengthIndicator.textContent = "";

    // Define criteria
    const minLength = 8;
    const minUpper = 1;
    const minLower = 1;
    const minNumbers = 1;
    const minSpecial = 1;

    let strength = 0;

    // Check length
    if (password.length >= minLength) {
      strength++;
    }

    // Check uppercase letters
    if (/[A-Z]/.test(password) && password.match(/[A-Z]/g) != null && password.match(/[A-Z]/g)!.length >= minUpper) {
      strength++;
    }

    // Check lowercase letters
    if (/[a-z]/.test(password) && password.match(/[a-z]/g) != null && password.match(/[a-z]/g)!.length >= minLower) {
      strength++;
    }

    // Check numbers
    if (/\d/.test(password) && password.match(/\d/g) != null && password.match(/\d/g)!.length >= minNumbers) {
      strength++;
    }

    // Check special characters
    if (/[^a-zA-Z0-9]/.test(password) && password.match(/[^a-zA-Z0-9]/g) != null && password.match(/[^a-zA-Z0-9]/g)!.length >= minSpecial) {
      strength++;
    }

    // Update strength indicator
    switch (strength) {
      case 0:
      case 1:
        strengthIndicator.textContent = "Weak";
        strengthIndicator.style.color = "red";
        break;
      case 2:
      case 3:
        strengthIndicator.textContent = "Medium";
        strengthIndicator.style.color = "orange";
        break;
      case 4:
      case 5:
        strengthIndicator.textContent = "Strong";
        strengthIndicator.style.color = "green";
        break;
      default:
        break;
    }
  }

  showError(element: HTMLInputElement | null, message: string): void {
    if (element) {
      const errorSpan = document.createElement("span");
      errorSpan.className = "error-message";
      errorSpan.textContent = message;
      element.parentElement?.appendChild(errorSpan);
    }
  }

  clearErrors(): void {
    const errors = document.querySelectorAll(".error-message");
    errors.forEach((error) => error.remove());
  }
}