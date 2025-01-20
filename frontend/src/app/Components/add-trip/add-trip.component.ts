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
    if (!this.tripName) {
      this.message = 'Trip name is required';
      return;
    }

    const tripData = {
      tripName: this.tripName
    };

    this.http.post<{ message: string }>('http://localhost:5134/api/trip/add-trip', tripData)
      .subscribe(response => {
        this.message = response.message;
        console.log(response);
      }, error => {
        this.message = error.error.message;
        console.error(error);
      });
  }
}