import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../navbar/navbar.component';
import { LoginComponent } from '../login/login.component';
import { SignupComponent } from '../signup/signup.component';
import { AddTripComponent } from '../add-trip/add-trip.component';
import { TripDashboard } from '../trip-dashboard/trip-dashboard.component';
import { ExpenseComponent } from '../expense/expense.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    RouterOutlet,
    NavbarComponent,
    LoginComponent,
    SignupComponent,
    AddTripComponent,
    TripDashboard,
    ExpenseComponent
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent { }