import { Component } from '@angular/core';

@Component({
  selector: 'app-expense',
  templateUrl: './expense.component.html',
  styleUrls: ['./expense.component.css']
})
export class ExpenseComponent {
  displayedColumns: string[] = ['date', 'category', 'comment', 'amount', 'actions'];
  expenses = [
    {
      expense_id: '1',
      date: new Date(),
      category: 'Food',
      comment: 'Lunch with clients',
      amount: 50.0,
    },
    {
      expense_id: '2',
      date: new Date(),
      category: 'Travel',
      comment: 'Taxi fare',
      amount: 25.0,
    },
  ];

  openAddExpenseDialog() {
    // Logic to open a dialog to add a new expense
    console.log('Add Expense Dialog Opened');
  }

  editExpense(expense: any) {
    // Logic to edit the selected expense
    console.log('Editing Expense:', expense);
  }

  deleteExpense(expenseId: string) {
    // Logic to delete the expense
    this.expenses = this.expenses.filter(expense => expense.expense_id !== expenseId);
    console.log('Deleted Expense with ID:', expenseId);
  }
}
