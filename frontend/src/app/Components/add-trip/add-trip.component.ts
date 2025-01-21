import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-add-trip',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './add-trip.component.html',
  styleUrls: ['./add-trip.component.css']
})
export class AddTripComponent {
  tripName: string = '';
  message: string = '';

  constructor(private http: HttpClient) {}

  onSubmit() {
    const userId = localStorage.getItem('user_id');
    if (!this.tripName || !userId) {
      this.message = 'Trip name and user ID are required';
      return;
    }

    const tripData = {
      tripName: this.tripName,
      userId: userId
    };

    this.http.post<{ message: string, tripId: string }>('http://localhost:5134/api/trip/add-trip', tripData)
      .subscribe(response => {
        this.message = response.message;
        console.log('Trip created successfully', response);
        // Optionally, redirect to another page
        // window.location.href = '/home';
      }, error => {
        this.message = error.error.message;
        console.error('Failed to create trip', error);
      });
  }
}