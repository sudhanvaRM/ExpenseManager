import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  usernameError: boolean = false;
  passwordError: boolean = false;
  message: string = '';

  constructor(private http: HttpClient, private router: Router) {}

  onSubmit() {
    this.usernameError = !this.username;
    this.passwordError = !this.password;

    if (!this.usernameError && !this.passwordError) {
      const loginData = {
        username: this.username,
        password: this.password
      };

      this.http.post<{ message: string, user_id: string }>('http://localhost:5134/api/login', loginData)
        .subscribe(response => {
          this.message = response.message;
          console.log('Login successful', response);
          localStorage.setItem('user_id', response.user_id);
          localStorage.setItem('username', this.username);
          localStorage.setItem('isLoggedIn', 'true');
         this.router.navigate(['/home']);
        }, error => {
          this.message = error.error.message;
          console.error('Login failed', error);
        });
    }
  }

  close() {
    this.router.navigate(['/home']);
  }

  navigateToSignup(){
    this.router.navigate(['/home/signup']);
  }
}