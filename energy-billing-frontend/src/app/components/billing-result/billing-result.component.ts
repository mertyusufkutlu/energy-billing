import { Component, OnInit, ViewChild } from '@angular/core';
import { BillingService } from '../../services/billing.service';
import { InvoiceResult } from '../../models/invoice-result.model';
import * as XLSX from 'xlsx';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-billing-result',
  templateUrl: './billing-result.component.html',
})
export class BillingResultComponent implements OnInit {
  @ViewChild('dt') dt!: Table;

  result!: InvoiceResult;
  tableData: { kalem: string; tutar: number }[] = [];

  constructor(private billingService: BillingService) {}

  ngOnInit(): void {
    this.loadResult();
  }

  loadResult(): void {
    const customerId = '3fa85f64-5717-4562-b3fc-2c963f66afa6';
    this.billingService.getInvoiceResult(customerId).subscribe({
      next: (data: InvoiceResult) => {
        this.result = data;
        this.prepareTableData();
      },
      error: (err) => console.error('Hata:', err),
    });
  }

  prepareTableData(): void {
    this.tableData = [
      { kalem: 'Enerji Bedeli', tutar: this.result.energyTotal },
      { kalem: 'Dağıtım Bedeli', tutar: this.result.distributionTotal },
      { kalem: 'BTV', tutar: this.result.btvTotal },
      { kalem: 'KDV', tutar: this.result.vatTotal },
      { kalem: 'Toplam', tutar: this.result.grandTotal },
    ];
  }

  exportExcel(): void {
    const worksheet = XLSX.utils.json_to_sheet(this.tableData);
    const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] };
    XLSX.writeFile(workbook, 'FaturaOzet.xlsx');
  }

  onGlobalFilter(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.dt.filterGlobal(input.value, 'contains');
  }

}
