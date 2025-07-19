import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InvoiceResult } from '../models/invoice-result.model';

@Injectable({
  providedIn: 'root',
})
export class BillingService {
  private apiUrl = 'http://localhost:5217/api/Invoice';

  constructor(private http: HttpClient) {}

  getInvoiceResult(customerId: string): Observable<InvoiceResult> {
    const body = { customerId };
    return this.http.post<InvoiceResult>(`${this.apiUrl}/calculate`, body);
  }
}
