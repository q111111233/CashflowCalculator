import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public cashflows;
  public pool;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  showTables() {
    this.http.get<CashFlow[]>(this.baseUrl + 'api/Loan').subscribe(result => {
      this.cashflows = result;
    }, error => console.error(error));
  }

  getPool() {
    this.http.get<CashFlow[]>(this.baseUrl + 'api/Loan/getPool').subscribe(result => {
      this.pool = result;
    }, error => console.error(error));
  }
  
}

interface CashFlow {
  Month: number[];
  Interest: number[];
  Principal: number[];
  RemBalance: number[];
}
