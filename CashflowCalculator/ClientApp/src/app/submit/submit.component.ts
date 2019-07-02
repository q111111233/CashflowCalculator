import { Component, OnInit, Inject, Output, EventEmitter } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-submit',
  templateUrl: './submit.component.html',
})
export class SubmitComponent{

  loan: Loan = new Loan;
  @Output() table = new EventEmitter();
  @Output() pool = new EventEmitter();
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  onSubmit(form) {
    this.http.post<Loan>(this.baseUrl + 'api/Loan', this.loan)
      .subscribe(
      res => {
        form.form.reset();
        this.table.emit(null);
        this.pool.emit(null);
      },
      err => {
        console.log(err);
      },
    )
  }
}

class Loan {
  balance: string;
  term: number;
  rate: number;
}
