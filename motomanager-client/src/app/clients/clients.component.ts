import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClientService } from '../services/client.service';
import { VehicleService } from '../services/vehicle.service';
import { Client, CreateClientRequest, UpdateClientRequest } from '../models/client.model';
import { Vehicle, CreateVehicleRequest } from '../models/vehicle.model';

@Component({
  selector: 'app-clients',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './clients.component.html',
  styleUrl: './clients.component.css'
})
export class ClientsComponent implements OnInit {
  clients: Client[] = [];
  loading = false;
  error: string | null = null;
  
  // Pagination
  currentPage = 1;
  pageSize = 20;
  totalCount = 0;
  totalPages = 0;
  
  showAddForm = false;
  editingClient: Client | null = null;
  
  // Vehicles
  viewingClientVehicles: Client | null = null;
  clientVehicles: Vehicle[] = [];
  showVehicleAddForm = false;
  editingVehicle: Vehicle | null = null;
  newVehicle: CreateVehicleRequest = { model: '', plate: '', clientId: 0 };
  
  newClient: CreateClientRequest = {
    naziv: '',
    adresa: '',
    grad: '',
    pib: null,
    telefon: '',
    email: ''
  };
  
  updatedClient: UpdateClientRequest = {
    id: 0,
    naziv: '',
    adresa: '',
    grad: '',
    pib: null,
    telefon: '',
    email: ''
  };

  constructor(
    private clientService: ClientService,
    private vehicleService: VehicleService
  ) { }

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients(): void {
    this.loading = true;
    this.error = null;
    this.clientService.getClientsPaged(this.currentPage, this.pageSize).subscribe({
      next: (pagedResult) => {
        this.clients = pagedResult.items || [];
        this.totalCount = pagedResult.totalCount || 0;
        this.totalPages = pagedResult.totalPages || 0;
        this.currentPage = pagedResult.currentPage || 1;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška prilikom učitavanja klijenata';
        this.loading = false;
        console.error('Error loading clients:', err);
      }
    });
  }

  showAddClientForm(): void {
    this.showAddForm = true;
    this.newClient = {
      naziv: '',
      adresa: '',
      grad: '',
      pib: null,
      telefon: '',
      email: ''
    };
  }

  cancelAdd(): void {
    this.showAddForm = false;
  }

  addClient(): void {
    if (!this.newClient.naziv || !this.newClient.adresa || !this.newClient.grad || 
        !this.newClient.telefon || !this.newClient.email) {
      alert('Popunite sva obavezna polja!');
      return;
    }

    this.clientService.createClient(this.newClient).subscribe({
      next: (client) => {
        this.clients.push(client);
        this.showAddForm = false;
        this.newClient = { naziv: '', adresa: '', grad: '', pib: null, telefon: '', email: '' };
      },
      error: (err) => {
        this.error = 'Greška prilikom dodavanja klijenta';
        console.error('Error adding client:', err);
      }
    });
  }

  editClient(client: Client): void {
    this.editingClient = client;
    this.updatedClient = {
      id: client.id,
      naziv: client.naziv,
      adresa: client.adresa,
      grad: client.grad,
      pib: client.pib,
      telefon: client.telefon,
      email: client.email
    };
  }

  cancelEdit(): void {
    this.editingClient = null;
  }

  updateClient(): void {
    if (!this.updatedClient.naziv || !this.updatedClient.adresa || !this.updatedClient.grad || 
        !this.updatedClient.telefon || !this.updatedClient.email) {
      alert('Popunite sva obavezna polja!');
      return;
    }

    this.clientService.updateClient(this.updatedClient.id, this.updatedClient).subscribe({
      next: (updated) => {
        const index = this.clients.findIndex(c => c.id === updated.id);
        if (index !== -1) {
          this.clients[index] = updated;
        }
        this.editingClient = null;
      },
      error: (err) => {
        this.error = 'Greška prilikom ažuriranja klijenta';
        console.error('Error updating client:', err);
      }
    });
  }

  deleteClient(id: number): void {
    if (!confirm('Da li ste sigurni da želite da obrišete ovog klijenta?')) {
      return;
    }

    this.clientService.deleteClient(id).subscribe({
      next: () => {
        this.clients = this.clients.filter(c => c.id !== id);
      },
      error: (err) => {
        this.error = 'Greška prilikom brisanja klijenta';
        console.error('Error deleting client:', err);
      }
    });
  }

  // Vehicle management functions
  showVehicles(client: Client): void {
    this.viewingClientVehicles = client;
    this.loadClientVehicles(client.id);
  }

  closeVehicles(): void {
    this.viewingClientVehicles = null;
    this.clientVehicles = [];
    this.showVehicleAddForm = false;
    this.editingVehicle = null;
  }

  loadClientVehicles(clientId: number): void {
    this.vehicleService.getAllVehicles().subscribe({
      next: (vehicles) => {
        this.clientVehicles = vehicles.filter(v => v.clientId === clientId);
      },
      error: (err) => {
        this.error = 'Greška prilikom učitavanja vozila';
        console.error('Error loading vehicles:', err);
      }
    });
  }

  showAddVehicleForm(): void {
    this.showVehicleAddForm = true;
    this.editingVehicle = null;
    this.newVehicle = { 
      model: '', 
      plate: '', 
      clientId: this.viewingClientVehicles?.id || 0 
    };
  }

  cancelAddVehicle(): void {
    this.showVehicleAddForm = false;
    this.newVehicle = { model: '', plate: '', clientId: 0 };
  }

  addVehicle(): void {
    if (!this.newVehicle.model || !this.newVehicle.plate || !this.viewingClientVehicles) {
      this.error = 'Model i tablica su obavezni!';
      return;
    }

    this.newVehicle.clientId = this.viewingClientVehicles.id;

    this.vehicleService.createVehicle(this.newVehicle).subscribe({
      next: (vehicle) => {
        this.clientVehicles.push(vehicle);
        this.showVehicleAddForm = false;
        this.newVehicle = { model: '', plate: '', clientId: 0 };
      },
      error: (err) => {
        this.error = 'Greška prilikom dodavanja vozila';
        console.error('Error adding vehicle:', err);
      }
    });
  }

  editVehicle(vehicle: Vehicle): void {
    this.editingVehicle = { ...vehicle };
    this.showVehicleAddForm = false;
  }

  cancelEditVehicle(): void {
    this.editingVehicle = null;
  }

  updateVehicle(): void {
    if (!this.editingVehicle) return;

    if (!this.editingVehicle.model || !this.editingVehicle.plate) {
      this.error = 'Model i tablica su obavezni!';
      return;
    }

    this.vehicleService.updateVehicle(this.editingVehicle.id, {
      model: this.editingVehicle.model,
      plate: this.editingVehicle.plate,
      clientId: this.editingVehicle.clientId
    }).subscribe({
      next: () => {
        const index = this.clientVehicles.findIndex(v => v.id === this.editingVehicle!.id);
        if (index !== -1) {
          this.clientVehicles[index] = { ...this.editingVehicle! };
        }
        this.editingVehicle = null;
      },
      error: (err) => {
        this.error = 'Greška prilikom ažuriranja vozila';
        console.error('Error updating vehicle:', err);
      }
    });
  }

  deleteVehicle(id: number): void {
    if (!confirm('Da li ste sigurni?')) return;

    this.vehicleService.deleteVehicle(id).subscribe({
      next: () => {
        this.clientVehicles = this.clientVehicles.filter(v => v.id !== id);
      },
      error: (err) => {
        this.error = 'Greška prilikom brisanja vozila';
        console.error('Error deleting vehicle:', err);
      }
    });
  }

  // Pagination methods
  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadClients();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadClients();
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadClients();
    }
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 5;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);
    
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }
}
