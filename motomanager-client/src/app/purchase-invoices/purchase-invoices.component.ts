import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PurchaseInvoiceService } from '../services/purchase-invoice.service';
import { ClientService } from '../services/client.service';
import { VehicleService } from '../services/vehicle.service';
import { PurchaseInvoice, CreatePurchaseInvoiceRequest, UpdatePurchaseInvoiceRequest } from '../models/purchase-invoice.model';
import { Client } from '../models/client.model';
import { Vehicle } from '../models/vehicle.model';

@Component({
  selector: 'app-purchase-invoices',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './purchase-invoices.component.html',
  styleUrl: './purchase-invoices.component.css'
})
export class PurchaseInvoicesComponent implements OnInit {
  invoices: PurchaseInvoice[] = [];
  filteredInvoices: PurchaseInvoice[] = [];
  clients: Client[] = [];
  vehicles: Vehicle[] = [];
  loading = false;
  error: string | null = null;
  
  // Filters
  filterDateFrom: string = '';
  filterDateTo: string = '';
  filterDobavljacId: number | null = null;
  filterVoziloId: number | null = null;
  
  showAddForm = false;
  editingInvoice: PurchaseInvoice | null = null;
  
  newInvoice: CreatePurchaseInvoiceRequest = {
    brojRacuna: '',
    datum: new Date(),
    dobavljacId: 0,
    voziloId: null,
    iznosNeto: 0,
    iznosPDV: 0,
    iznosBruto: 0
  };
  
  updatedInvoice: UpdatePurchaseInvoiceRequest = {
    id: 0,
    brojRacuna: '',
    datum: new Date(),
    dobavljacId: 0,
    voziloId: null,
    iznosNeto: 0,
    iznosPDV: 0,
    iznosBruto: 0
  };

  constructor(
    private purchaseInvoiceService: PurchaseInvoiceService,
    private clientService: ClientService,
    private vehicleService: VehicleService
  ) { }

  ngOnInit(): void {
    this.loadInvoices();
    this.loadClients();
    this.loadVehicles();
  }

  loadInvoices(): void {
    this.loading = true;
    this.error = null;
    this.purchaseInvoiceService.getAllPurchaseInvoices().subscribe({
      next: (data) => {
        this.invoices = data;
        this.filteredInvoices = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška prilikom učitavanja računa dobavljača';
        this.loading = false;
        console.error('Error loading purchase invoices:', err);
      }
    });
  }

  loadClients(): void {
    this.clientService.getAllClients().subscribe({
      next: (data) => {
        this.clients = data;
      },
      error: (err) => {
        console.error('Error loading clients:', err);
      }
    });
  }

  loadVehicles(): void {
    this.vehicleService.getAllVehicles().subscribe({
      next: (data) => {
        this.vehicles = data;
      },
      error: (err) => {
        console.error('Error loading vehicles:', err);
      }
    });
  }

  showAddInvoiceForm(): void {
    this.showAddForm = true;
    this.newInvoice = {
      brojRacuna: '',
      datum: new Date(),
      dobavljacId: 0,
      voziloId: null,
      iznosNeto: 0,
      iznosPDV: 0,
      iznosBruto: 0
    };
  }

  cancelAdd(): void {
    this.showAddForm = false;
  }

  calculateAmounts(): void {
    if (this.showAddForm) {
      this.newInvoice.iznosBruto = this.newInvoice.iznosNeto + this.newInvoice.iznosPDV;
    } else if (this.editingInvoice) {
      this.updatedInvoice.iznosBruto = this.updatedInvoice.iznosNeto + this.updatedInvoice.iznosPDV;
    }
  }

  addInvoice(): void {
    if (!this.newInvoice.brojRacuna || this.newInvoice.dobavljacId === 0) {
      alert('Popunite sva obavezna polja!');
      return;
    }

    this.purchaseInvoiceService.createPurchaseInvoice(this.newInvoice).subscribe({
      next: (invoice) => {
        this.invoices.push(invoice);
        this.showAddForm = false;
        this.newInvoice = {
          brojRacuna: '',
          datum: new Date(),
          dobavljacId: 0,
          voziloId: null,
          iznosNeto: 0,
          iznosPDV: 0,
          iznosBruto: 0
        };
      },
      error: (err) => {
        this.error = 'Greška prilikom dodavanja računa dobavljača';
        console.error('Error adding purchase invoice:', err);
      }
    });
  }

  editInvoice(invoice: PurchaseInvoice): void {
    this.editingInvoice = invoice;
    this.updatedInvoice = {
      id: invoice.id,
      brojRacuna: invoice.brojRacuna,
      datum: invoice.datum,
      dobavljacId: invoice.dobavljacId,
      voziloId: invoice.voziloId,
      iznosNeto: invoice.iznosNeto,
      iznosPDV: invoice.iznosPDV,
      iznosBruto: invoice.iznosBruto
    };
  }

  cancelEdit(): void {
    this.editingInvoice = null;
  }

  updateInvoice(): void {
    if (!this.updatedInvoice.brojRacuna || this.updatedInvoice.dobavljacId === 0) {
      alert('Popunite sva obavezna polja!');
      return;
    }

    this.purchaseInvoiceService.updatePurchaseInvoice(this.updatedInvoice.id, this.updatedInvoice).subscribe({
      next: (updated) => {
        const index = this.invoices.findIndex(i => i.id === updated.id);
        if (index !== -1) {
          this.invoices[index] = updated;
        }
        this.editingInvoice = null;
      },
      error: (err) => {
        this.error = 'Greška prilikom ažuriranja računa dobavljača';
        console.error('Error updating purchase invoice:', err);
      }
    });
  }

  deleteInvoice(id: number): void {
    if (!confirm('Da li ste sigurni da želite da obrišete ovaj račun dobavljača?')) {
      return;
    }

    this.purchaseInvoiceService.deletePurchaseInvoice(id).subscribe({
      next: () => {
        this.invoices = this.invoices.filter(i => i.id !== id);
      },
      error: (err) => {
        this.error = 'Greška prilikom brisanja računa dobavljača';
        console.error('Error deleting purchase invoice:', err);
      }
    });
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('sr-RS');
  }

  getTotalNeto()filteredInvoices.reduce((sum, invoice) => sum + invoice.iznosNeto, 0);
  }

  getTotalPDV(): number {
    return this.filteredInvoices.reduce((sum, invoice) => sum + invoice.iznosPDV, 0);
  }

  getTotalBruto(): number {
    return this.filteredInvoices.reduce((sum, invoice) => sum + invoice.iznosBruto, 0);
  }

  applyFilters(): void {
    this.filteredInvoices = this.invoices.filter(invoice => {
      // Datum od filter
      if (this.filterDateFrom) {
        const invoiceDate = new Date(invoice.datum);
        const dateFrom = new Date(this.filterDateFrom);
        if (invoiceDate < dateFrom) {
          return false;
        }
      }

      // Datum do filter
      if (this.filterDateTo) {
        const invoiceDate = new Date(invoice.datum);
        const dateTo = new Date(this.filterDateTo);
        if (invoiceDate > dateTo) {
          return false;
        }
      }

      // Dobavljač filter
      if (this.filterDobavljacId !== null && this.filterDobavljacId > 0) {
        if (invoice.dobavljacId !== this.filterDobavljacId) {
          return false;
        }
      }

      // Vozilo filter
      if (this.filterVoziloId !== null && this.filterVoziloId > 0) {
        if (invoice.voziloId !== this.filterVoziloId) {
          return false;
        }
      }

      return true;
    });
  }

  clearFilters(): void {
    this.filterDateFrom = '';
    this.filterDateTo = '';
    this.filterDobavljacId = null;
    this.filterVoziloId = null;
    this.filteredInvoices = this.invoices
    return this.invoices.reduce((sum, invoice) => sum + invoice.iznosBruto, 0);
  }
}
