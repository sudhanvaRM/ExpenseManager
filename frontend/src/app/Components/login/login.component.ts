import { Component } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  usernameError: boolean = false;
  passwordError: boolean = false;
  message: string = '';

  constructor(private http: HttpClient) {}

  onSubmit() {
    this.usernameError = !this.username;
    this.passwordError = !this.password;

    if (!this.usernameError && !this.passwordError) {
      const loginData = {
        username: this.username,
        password: this.password
      };

      this.http.post<{ message: string }>('http://localhost:5134/api/login', loginData)
        .subscribe(response => {
          this.message = response.message;
          console.log('Login successful', response);
        }, error => {
          this.message = error.error.message;
          console.error('Login failed', error);
        });
    }
  }
}