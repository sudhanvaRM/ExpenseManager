import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-trip-dashboard',
  templateUrl: './trip-dashboard.component.html',
  imports: [CommonModule, FormsModule],
  styleUrls: ['./trip-dashboard.component.css'],
})
export class TripDashboardComponent implements OnInit {
  userId: string = ''; // Replace with dynamic user ID as needed
  trips: any[] = [];
  selectedTripDebts: any = null;
  selectedTripName: string = '';
  isLoading: boolean = false;
  message: string = '';

  // Add Participants Logic
  participantsNotInTrip: any[] = [];
  selectedUserId: string = '';
  selectedTripId: string = '';

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.userId = localStorage.getItem('user_id') || '';
    this.getTrips();
  }

  // Fetch trips for the user
  getTrips(): void {
    this.isLoading = true;
    this.http.get(`http://localhost:5134/api/trip/trip-details/${this.userId}`).subscribe(
      (data: any) => {
        this.message = data.message;
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
      TripId: tripId,
    };

    this.http.post('http://localhost:5134/api/trip/fetch-trip-debts', payload).subscribe(
      (data: any) => {
        console.log('Fetched trip debts:', data);
        this.selectedTripDebts = {
          OwesToOthers: data.owesToOthers, // Correct the response structure
          OwedByOthers: data.owedByOthers, // Correct the response structure
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
    if (!confirm('Are you sure you want to settle this expense?')) {
      return;
    }

    const requestPayload = {
      TripId: tripId,
      DebtorId: this.userId,
      CreditorId: creditorId,
    };

    console.log('Settling expense:', requestPayload);

    this.isLoading = true;
    this.http.post('http://localhost:5134/api/trip/settle-debt', requestPayload).subscribe(
      (response: any) => {
        alert('Expense settled successfully!');
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

  // Fetch users not in the trip
  fetchParticipantsNotInTrip(tripId: string): void {
    this.isLoading = true;
    this.selectedTripId = tripId;
    this.http.get(`http://localhost:5134/api/NotInTrip/${tripId}`).subscribe(
      (data: any) => {
        console.log('Participants not in trip:', data);
        this.participantsNotInTrip = data;
        this.isLoading = false;
      },
      (error) => {
        console.error('Error fetching participants not in trip', error);
        this.isLoading = false;
      }
    );
  }

  // Add participant to the trip
  addParticipant(): void {
    if (!this.selectedUserId || !this.selectedTripId) {
      alert('Please select a participant to add.');
      return;
    }

    const addTripPayload = {
      TripId: this.selectedTripId,
      UserId: this.selectedUserId,
    };

    this.isLoading = true;
    this.http.post('http://localhost:5134/api/trip/add-person-to-trip', addTripPayload).subscribe(
      (response: any) => {
        alert('Participant added successfully!');
        // Reload the participants not in the trip
        this.fetchParticipantsNotInTrip(this.selectedTripId);
        this.isLoading = false;
      },
      (error) => {
        console.error('Error adding participant to trip', error);
        alert('Failed to add participant. Please try again.');
        this.isLoading = false;
      }
    );
  }

  navigateToAddParticipant(tripId: string): void {
    this.router.navigate(['/home/add-participant', tripId]); // Pass the tripId as a parameter
  }
}
