import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-trip-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './trip-dashboard.component.html',
  styleUrls: ['./trip-dashboard.component.css']
})
export class TripDashboard implements OnInit {
  trips: any[] = [];
  message: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadTrips();
  }

  loadTrips() {
    const userId = localStorage.getItem('user_id');
    if (!userId) {
      this.message = 'User ID not found in local storage';
      return;
    }

    this.http.get<any[]>(`http://localhost:5134/api/trip/user-trips/${userId}`)
      .subscribe(response => {
        this.trips = response;
      }, error => {
        this.message = error.error.message;
        console.error('Failed to load trips', error);
      });
  }

  addExpense(tripId: string) {
    // Navigate to add expense page or open a modal
    console.log('Add expense for trip', tripId);
  }

  addParticipant(tripId: string) {
    // Navigate to add participant page or open a modal
    console.log('Add participant for trip', tripId);
  }
}