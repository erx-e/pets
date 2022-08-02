import { Component, OnInit } from "@angular/core";
import { authUser } from "src/app/models/user.model";
import { AuthService } from "src/app/services/auth.service";
import { Router } from "@angular/router";
import {
  FormControl,
  Validators,
  FormGroup,
  FormBuilder,
} from "@angular/forms";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit {
  form: FormGroup;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.buildForm();
  }

  user: authUser;
  showPassword: boolean = false;
  wrongUser: boolean = false;

  changePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  login() {
    if (this.form.valid) {
      this.user = this.form.value;
      this.authService.login(this.user).subscribe(
        () => {
          this.authService.getCurrentUser();
          this.router.navigate(["/home"]);
        },
        (error: string) => {
          console.log(error);
          this.wrongUser = true;
        }
      );
    }
    this.form.markAllAsTouched();
  }

  getUser() {
    this.authService.getProfile(undefined);
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required]],
    });
  }

  get emailField() {
    return this.form.get("email");
  }

  get emailFieldValid() {
    return this.emailField.touched && this.emailField.valid;
  }

  get emailFieldInvalid() {
    return this.emailField.touched && this.emailField.invalid;
  }

  get passwordField() {
    return this.form.get("password");
  }
  get passwordFieldValid() {
    return this.passwordField.touched && this.passwordField.valid;
  }

  get passwordFieldInvalid() {
    return this.passwordField.touched && this.passwordField.invalid;
  }
}
