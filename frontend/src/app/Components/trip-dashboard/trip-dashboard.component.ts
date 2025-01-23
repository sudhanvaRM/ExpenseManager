import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-trip-dashboard',
  templateUrl: './trip-dashboard.component.html',
  imports: [CommonModule], 
  styleUrls: ['./trip-dashboard.component.css']
})
export class TripDashboardComponent implements OnInit {
  userId: string = ''; // Replace with dynamic user ID as needed
  trips: any[] = [];
  selectedTripDebts: any = null;
  selectedTripName: string = '';
  isLoading: boolean = false;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.userId = localStorage.getItem('user_id') || '';
    this.getTrips();
  }

  // Fetch trips for the user
  getTrips(): void {
    this.isLoading = true;
    this.http.get(`http://localhost:5134/api/trip/trip-details/${this.userId}`).subscribe(
      (data: any) => {
        // console.log('Fetched trips:', data);
        this.trips = data;
        this.isLoading = false;
      },
      (error) => {
        console.error('Error fetching trips', error);
        this.isLoading = false;
      }
    );
  }

  // Fetch debts for a selected trip
  viewTripDetails(tripId: string): void {
    this.isLoading = true;
    const payload = {
      UserId: this.userId,
      TripId: tripId
    };

    console.log('Fetching trip debts for:', payload);

    this.http.post('http://localhost:5134/api/trip/fetch-trip-debts', payload).subscribe(
      (data: any) => {
        console.log('Fetched trip debts:', data);
        // Mock response syncing
        this.selectedTripDebts = {
          OwesToOthers: data.owesToOthers, // Correct the response structure
          OwedByOthers: data.owedByOthers  // Correct the response structure
        };
        this.selectedTripName = this.trips.find((trip) => trip.TripId === tripId)?.TripName || '';
        this.isLoading = false;
      },
      (error) => {
        console.error('Error fetching trip debts', error);
        this.isLoading = false;
      }
    );
  }

  // Handle settling expense
  settleExpense(creditorId: string): void {
    alert(`Settle expense with Creditor ID: ${creditorId}`);
    // Implement settle logic here
  }
}
