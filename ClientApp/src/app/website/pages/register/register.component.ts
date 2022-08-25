import { Component, OnInit } from "@angular/core";
import { CreateUserDTO } from "src/app/models/user.model";
import { UserService } from "src/app/services/user.service";
import { Validators, FormGroup, FormBuilder } from "@angular/forms";
import { MyValidators } from "src/app/validators/validators";
import { Router } from "@angular/router";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.scss"],
})
export class RegisterComponent implements OnInit {
  form: FormGroup;

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {
    this.buildForm();
  }

  ngOnInit(): void {}

  showPassword: boolean = false;
  showConfirmPassword: boolean = false;

  changePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  changeConfirmPasswordVisibility() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  user: CreateUserDTO;
  emailAlreadyRegistered: boolean = false;

  createUser() {
    if (this.form.valid) {
      this.user = this.form.value;
      this.userService.create(this.user).subscribe(
        () => {
          this.router.navigate(["home"]);
        },
        (error: string) => {
          console.log(error);
          if (error == "Email already registered") {
            this.emailAlreadyRegistered = true;
          }
        }
      );
    }
    this.form.markAllAsTouched();
  }

  private buildForm() {
    this.form = this.formBuilder.group(
      {
        name: [
          "",
          [
            Validators.required,
            Validators.pattern(/^([Aa-zA-ZáéíóúÁÉÍÓÚÑñ]{2,}\s?){2,4}$/),
          ],
        ],
        email: ["", [Validators.required, Validators.email]],
        password: [
          "",
          [
            Validators.required,
            Validators.minLength(6),
            Validators.pattern(
              /^((?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9]).{6,})\S$/
            ),
          ],
        ],
        confirmPassword: ["", Validators.required],
      },
      {
        validators: MyValidators.matchPassword,
      }
    );
  }

  get nameField() {
    return this.form.get("name");
  }

  get nameFieldValid() {
    return this.nameField.touched && this.nameField.valid;
  }

  get nameFieldInvalid() {
    return this.nameField.touched && this.nameField.invalid;
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

  get confirmPasswordField() {
    return this.form.get("confirmPassword");
  }
  get confirmPasswordFieldValid() {
    return this.confirmPasswordField.touched && this.confirmPasswordField.valid;
  }

  get confirmPasswordFieldInvalid() {
    return (
      this.confirmPasswordField.touched && this.confirmPasswordField.invalid
    );
  }
}
