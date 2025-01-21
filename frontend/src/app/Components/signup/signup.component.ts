import { Component } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule,Router } from '@angular/router';


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

  constructor(private http: HttpClient, private router: Router) {}

  onSubmit() {
    this.usernameError = !this.username;
    this.passwordError = !this.password;
    this.confirmPasswordError = this.password !== this.confirmPassword;

    if (!this.usernameError && !this.passwordError && !this.confirmPasswordError) {
      const signupData = {
        username: this.username,
        password: this.password
      };

      this.http.post<{ message: string,user_id: string }>('http://localhost:5134/api/signup', signupData)
        .subscribe(response => {
          this.message = response.message;
          console.log( response);
          localStorage.setItem('user_id', response.user_id);
          //window.location.href = '/home';   
          
        }, error => {
          this.message = error.error.message;
          console.error( error);
        });
    }
  }

  close() {
    this.router.navigate(['/home']);
  }

  navigateToLogin() {
    this.router.navigate(['/home/login']);
  }
}