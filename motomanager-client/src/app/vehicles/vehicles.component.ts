import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { VehicleService } from '../services/vehicle.service';
import { ClientService } from '../services/client.service';
import { Vehicle, CreateVehicleRequest } from '../models/vehicle.model';
import { Client } from '../models/client.model';

@Component({
  selector: 'app-vehicles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './vehicles.component.html',
  styleUrl: './vehicles.component.css'
})
export class VehiclesComponent implements OnInit {
  vehicles: Vehicle[] = [];
  clients: Client[] = [];
  newVehicle: CreateVehicleRequest = { model: '', plate: '', clientId: 0 };
  editingVehicle: Vehicle | null = null;
  showAddForm = false;
  loading = false;
  error: string | null = null;

  constructor(
    private vehicleService: VehicleService,
    private clientService: ClientService
  ) { }

  ngOnInit(): void {
    this.loadVehicles();
    this.loadClients();
  }

  loadClients(): void {
    this.clientService.getAllClients().subscribe({
      next: (data) => this.clients = data,
      error: (err) => console.error('Error loading clients:', err)
    });
  }

  loadVehicles(): void {
    this.loading = true;
    this.error = null;
    this.vehicleService.getAllVehicles().subscribe({
      next: (data) => {
        this.vehicles = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška pri učitavanju vozila: ' + err.message;
        this.loading = false;
      }
    });
  }

  addVehicle(): void {
    if (!this.newVehicle.model || !this.newVehicle.plate || this.newVehicle.clientId <= 0) {
      this.error = 'Model, tablica i klijent su obavezni!';
      return;
    }

    this.loading = true;
    this.error = null;
    this.vehicleService.createVehicle(this.newVehicle).subscribe({
      next: (vehicle) => {
        this.vehicles.push(vehicle);
        this.newVehicle = { model: '', plate: '', clientId: 0 };
        this.showAddForm = false;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška pri dodavanju vozila: ' + err.message;
        this.loading = false;
      }
    });
  }

  showAddVehicleForm(): void {
    this.showAddForm = true;
    this.editingVehicle = null;
    this.error = null;
  }

  cancelAdd(): void {
    this.showAddForm = false;
    this.newVehicle = { model: '', plate: '', clientId: 0 };
    this.error = null;
  }

  editVehicle(vehicle: Vehicle): void {
    this.editingVehicle = { ...vehicle };
    this.showAddForm = false;
  }

  updateVehicle(): void {
    if (!this.editingVehicle) return;

    if (!this.editingVehicle.model || !this.editingVehicle.plate || this.editingVehicle.clientId <= 0) {
      this.error = 'Model, tablica i klijent su obavezni!';
      return;
    }

    this.loading = true;
    this.error = null;
    this.vehicleService.updateVehicle(this.editingVehicle.id, {
      model: this.editingVehicle.model,
      plate: this.editingVehicle.plate,
      clientId: this.editingVehicle.clientId
    }).subscribe({
      next: () => {
        const index = this.vehicles.findIndex(v => v.id === this.editingVehicle!.id);
        if (index !== -1) {
          this.vehicles[index] = { ...this.editingVehicle! };
        }
        this.editingVehicle = null;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška pri ažuriranju vozila: ' + err.message;
        this.loading = false;
      }
    });
  }

  cancelEdit(): void {
    this.editingVehicle = null;
    this.error = null;
  }

  deleteVehicle(id: number): void {
    if (!confirm('Da li ste sigurni da želite da obrišete ovo vozilo?')) return;

    this.loading = true;
    this.error = null;
    this.vehicleService.deleteVehicle(id).subscribe({
      next: () => {
        this.vehicles = this.vehicles.filter(v => v.id !== id);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška pri brisanju vozila: ' + err.message;
        this.loading = false;
      }
    });
  }
}
