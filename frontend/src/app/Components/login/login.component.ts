// filepath: /c:/Users/26301/Desktop/ExpenseManager11/ExpenseManager/frontend/src/app/components/login/login.component.ts
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  usernameError: boolean = false;
  passwordError: boolean = false;

  constructor(private http: HttpClient) {}

  onSubmit() {
    this.usernameError = !this.username;
    this.passwordError = !this.password;

    if (!this.usernameError && !this.passwordError) {
      const loginData = {
        username: this.username,
        password: this.password
      };

      this.http.post('/api/login', loginData)
        .subscribe(response => {
          console.log('Login successful', response);
        }, error => {
          console.error('Login failed', error);
        });
    }
  }
}