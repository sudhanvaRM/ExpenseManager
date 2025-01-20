import { Component } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, RouterModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent {
  username: string = '';
  password: string = '';
  confirmPassword: string = '';
  usernameError: boolean = false;
  passwordError: boolean = false;
  confirmPasswordError: boolean = false;
  message: string = '';

  constructor(private http: HttpClient) {}

  onSubmit() {
    this.usernameError = !this.username;
    this.passwordError = !this.password;
    this.confirmPasswordError = this.password !== this.confirmPassword;

    if (!this.usernameError && !this.passwordError && !this.confirmPasswordError) {
      const signupData = {
        username: this.username,
        password: this.password
      };

      this.http.post<{ message: string }>('http://localhost:5134/api/signup', signupData)
        .subscribe(response => {
          this.message = response.message;
          console.log( response);
        }, error => {
          this.message = error.error.message;
          console.error( error);
        });
    }
  }
}