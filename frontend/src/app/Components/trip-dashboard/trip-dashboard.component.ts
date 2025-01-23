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
  settleExpense(creditorId: string, tripId: string): void {
  // Confirm action
  if (!confirm('Are you sure you want to settle this expense?')) {
    return;
  }


  const requestPayload = {
    TripId: tripId,
    DebtorId: this.userId, // Assuming `currentUser` holds the logged-in user details
    CreditorId: creditorId,
  };

  console.log('Settling expense:', requestPayload);

  // Call the API to settle the debt
  this.isLoading = true;
  this.http.post('http://localhost:5134/api/trip/settle-debt', requestPayload).subscribe(
    (response: any) => {
      alert('Expense settled successfully!');
      // Reload the trip details to reflect changes

      this.viewTripDetails(this.selectedTripDebts.TripId);
      this.isLoading = false;
    },
    (error) => {
      console.error('Error settling expense', error);
      alert('Failed to settle expense. Please try again later.');
      this.isLoading = false;
    }
  );
}
  
}
