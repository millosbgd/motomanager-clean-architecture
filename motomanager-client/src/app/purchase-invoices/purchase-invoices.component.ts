import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PurchaseInvoiceService } from '../services/purchase-invoice.service';
import { ClientService } from '../services/client.service';
import { VehicleService } from '../services/vehicle.service';
import { SektorService } from '../services/sektor.service';
import { KorisnikService } from '../services/korisnik.service';
import { AuthService } from '@auth0/auth0-angular';
import { PurchaseInvoice, CreatePurchaseInvoiceRequest, UpdatePurchaseInvoiceRequest } from '../models/purchase-invoice.model';
import { Client } from '../models/client.model';
import { Vehicle } from '../models/vehicle.model';
import { Sektor } from '../models/sektor.model';
import { ExportDialogComponent, ExportFilters } from '../export-dialog/export-dialog.component';

@Component({
  selector: 'app-purchase-invoices',
  standalone: true,
  imports: [CommonModule, FormsModule, ExportDialogComponent],
  templateUrl: './purchase-invoices.component.html',
  styleUrl: './purchase-invoices.component.css'
})
export class PurchaseInvoicesComponent implements OnInit {
  invoices: PurchaseInvoice[] = [];
  filteredInvoices: PurchaseInvoice[] = [];
  clients: Client[] = [];
  vehicles: Vehicle[] = [];
  sektori: Sektor[] = [];
  loading = false;
  error: string | null = null;
  
  // Filters
  showFilters = false;
  filterDateFrom: string = '';
  filterDateTo: string = '';
  filterDobavljacId: number | null = null;
  filterVoziloId: number | null = null;
  
  showAddForm = false;
  showExportDialog = false;
  editingInvoice: PurchaseInvoice | null = null;
  openDropdownId: number | null = null;
  
  newInvoice: CreatePurchaseInvoiceRequest = {
    brojRacuna: '',
    datum: new Date(),
    dobavljacId: 0,
    voziloId: null,
    iznosNeto: 0,
    iznosPDV: 0,
    iznosBruto: 0,
    sektorId: null
  };
  
  updatedInvoice: UpdatePurchaseInvoiceRequest = {
    id: 0,
    brojRacuna: '',
    datum: new Date(),
    dobavljacId: 0,
    voziloId: null,
    iznosNeto: 0,
    iznosPDV: 0,
    iznosBruto: 0,
    sektorId: null
  };

  constructor(
    private purchaseInvoiceService: PurchaseInvoiceService,
    private clientService: ClientService,
    private vehicleService: VehicleService,
    private sektorService: SektorService,
    private korisnikService: KorisnikService,
    public auth: AuthService
  ) { }

  ngOnInit(): void {
    this.loadInvoices();
    this.loadClients();
    this.loadVehicles();
    this.loadSektori();
  }

  loadInvoices(): void {
    this.loading = true;
    this.error = null;
    this.purchaseInvoiceService.getAllPurchaseInvoices(
      this.filterDateFrom || undefined,
      this.filterDateTo || undefined,
      this.filterDobavljacId,
      this.filterVoziloId
    ).subscribe({
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

  loadSektori(): void {
    this.sektorService.getAll().subscribe({
      next: (data) => {
        this.sektori = data;
      },
      error: (err) => {
        console.error('Error loading sektori:', err);
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
      iznosBruto: 0,
      sektorId: null
    };
    
    // Automatski postavi sektorId iz ulogovanog korisnika
    this.auth.user$.subscribe(user => {
      if (user?.sub) {
        this.korisnikService.getById(user.sub).subscribe({
          next: (korisnik) => {
            if (korisnik.sektorId) {
              this.newInvoice.sektorId = korisnik.sektorId;
            }
          },
          error: (err) => {
            console.error('Error loading user sektor:', err);
          }
        });
      }
    });
  }

  cancelAdd(): void {
    this.showAddForm = false;
  }

  toggleDropdown(invoiceId: number): void {
    this.openDropdownId = this.openDropdownId === invoiceId ? null : invoiceId;
  }

  closeDropdown(): void {
    this.openDropdownId = null;
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
          iznosBruto: 0,
          sektorId: null
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
      iznosBruto: invoice.iznosBruto,
      sektorId: invoice.sektorId
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
        this.filteredInvoices = this.filteredInvoices.filter(i => i.id !== id);
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

  getTotalNeto(): number {
    return this.filteredInvoices.reduce((sum, invoice) => sum + invoice.iznosNeto, 0);
  }

  getTotalPDV(): number {
    return this.filteredInvoices.reduce((sum, invoice) => sum + invoice.iznosPDV, 0);
  }

  getTotalBruto(): number {
    return this.filteredInvoices.reduce((sum, invoice) => sum + invoice.iznosBruto, 0);
  }

  toggleFilters(): void {
    this.showFilters = !this.showFilters;
  }

  applyFilters(): void {
    // Server-side filtering - reload data from API with filters
    this.loadInvoices();
  }

  clearFilters(): void {
    this.filterDateFrom = '';
    this.filterDateTo = '';
    this.filterDobavljacId = null;
    this.filterVoziloId = null;
    this.loadInvoices();
  }

  openExportDialog(): void {
    this.showExportDialog = true;
  }

  closeExportDialog(): void {
    this.showExportDialog = false;
  }

  onExport(filters: ExportFilters): void {
    this.purchaseInvoiceService.exportToExcel(
      filters.datumOd,
      filters.datumDo,
      filters.dobavljacId,
      filters.voziloId
    ).subscribe({
      next: (blob) => {
        // Kreiranje linka za download
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `Racuni_dobavljaca_${new Date().toISOString().slice(0, 10)}.xlsx`;
        link.click();
        window.URL.revokeObjectURL(url);
        
        this.showExportDialog = false;
      },
      error: async (err) => {
        console.error('Error exporting to Excel:', err);
        
        // Pokušaj da pročitaš response body za detaljnu poruku
        if (err.error instanceof Blob) {
          const text = await err.error.text();
          console.error('Backend error response:', text);
          this.error = `Greška prilikom exporta: ${text}`;
        } else if (err.error && typeof err.error === 'string') {
          console.error('Backend error response:', err.error);
          this.error = `Greška prilikom exporta: ${err.error}`;
        } else {
          this.error = 'Greška prilikom exporta u Excel';
        }
      }
    });
  }
}
