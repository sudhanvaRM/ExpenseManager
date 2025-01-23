import { Component } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { LoginComponent } from '../login/login.component';
import { SignupComponent } from '../signup/signup.component';
import { AddTripComponent } from '../add-trip/add-trip.component';
import { TripDashboardComponent } from '../trip-dashboard/trip-dashboard.component';
import { AddExpenseComponent } from '../add-expense/add-expense.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    NavbarComponent,
    LoginComponent,
    SignupComponent,
    AddTripComponent,
    TripDashboardComponent,
    RouterOutlet,
    AddExpenseComponent
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent { }
