import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ServiceOrderService } from '../services/service-order.service';
import { ServiceOrderLaborService } from '../services/service-order-labor.service';
import { ServiceOrderMaterialService } from '../services/service-order-material.service';
import { ClientService } from '../services/client.service';
import { VehicleService } from '../services/vehicle.service';
import { MaterialService } from '../services/material.service';
import { ServiceOrder, CreateServiceOrderRequest, UpdateServiceOrderRequest } from '../models/service-order.model';
import { ServiceOrderLabor, CreateServiceOrderLaborRequest } from '../models/service-order-labor.model';
import { ServiceOrderMaterial, CreateServiceOrderMaterialRequest } from '../models/service-order-material.model';
import { Client } from '../models/client.model';
import { Vehicle } from '../models/vehicle.model';
import { Material } from '../models/material.model';

@Component({
  selector: 'app-service-orders',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './service-orders.component.html',
  styleUrl: './service-orders.component.css'
})
export class ServiceOrdersComponent implements OnInit {
  serviceOrders: ServiceOrder[] = [];
  clients: Client[] = [];
  vehicles: Vehicle[] = [];
  allMaterials: Material[] = [];
  loading = false;
  error: string | null = null;
  
  showAddForm = false;
  editingOrder: ServiceOrder | null = null;
  activeTab: 'info' | 'labor' | 'material' = 'info';
  
  // Labor & Material lists
  labors: ServiceOrderLabor[] = [];
  materials: ServiceOrderMaterial[] = [];
  
  newLabor: CreateServiceOrderLaborRequest = {
    serviceOrderId: 0,
    opisRadova: '',
    ukupnoVreme: 0,
    cena: 0
  };
  
  newMaterial: CreateServiceOrderMaterialRequest = {
    serviceOrderId: 0,
    materialId: 0,
    kolicina: 0,
    jedinicnaCena: 0,
    ukupnaCena: 0
  };
  
  newOrder: CreateServiceOrderRequest = {
    brojNaloga: '',
    datum: new Date().toISOString().substring(0, 10),
    clientId: 0,
    vehicleId: 0,
    opisRada: '',
    kilometraza: 0
  };
  
  updatedOrder: UpdateServiceOrderRequest = {
    id: 0,
    brojNaloga: '',
    datum: '',
    clientId: 0,
    vehicleId: 0,
    opisRada: '',
    kilometraza: 0
  };

  constructor(
    private serviceOrderService: ServiceOrderService,
    private laborService: ServiceOrderLaborService,
    private materialService: ServiceOrderMaterialService,
    private clientService: ClientService,
    private vehicleService: VehicleService,
    private materialMasterService: MaterialService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;
    
    Promise.all([
      this.serviceOrderService.getAllServiceOrders().toPromise(),
      this.clientService.getAllClients().toPromise(),
      this.vehicleService.getAllVehicles().toPromise(),
      this.materialMasterService.getAllMaterials().toPromise()
    ]).then(([orders, clients, vehicles, materials]) => {
      this.serviceOrders = orders || [];
      this.clients = clients || [];
      this.vehicles = vehicles || [];
      this.allMaterials = materials || [];
      this.loading = false;
    }).catch(err => {
      this.error = 'Greška prilikom učitavanja podataka';
      this.loading = false;
      console.error('Error loading data:', err);
    });
  }

  showAddOrderForm(): void {
    this.showAddForm = true;
    this.newOrder = {
      brojNaloga: '',
      datum: new Date().toISOString().substring(0, 10),
      clientId: 0,
      vehicleId: 0,
      opisRada: '',
      kilometraza: 0
    };
  }

  cancelAdd(): void {
    this.showAddForm = false;
  }

  addOrder(): void {
    if (!this.newOrder.brojNaloga || !this.newOrder.datum || this.newOrder.clientId === 0 || 
        this.newOrder.vehicleId === 0 || !this.newOrder.opisRada) {
      alert('Popunite sva obavezna polja!');
      return;
    }

    this.serviceOrderService.createServiceOrder(this.newOrder).subscribe({
      next: (order) => {
        this.serviceOrders.push(order);
        this.showAddForm = false;
      },
      error: (err) => {
        this.error = 'Greška prilikom dodavanja servisnog naloga';
        console.error('Error adding service order:', err);
      }
    });
  }

  editOrder(order: ServiceOrder): void {
    this.editingOrder = order;
    this.activeTab = 'info';
    this.updatedOrder = {
      id: order.id,
      brojNaloga: order.brojNaloga,
      datum: order.datum.substring(0, 10),
      clientId: order.clientId,
      vehicleId: order.vehicleId,
      opisRada: order.opisRada,
      kilometraza: order.kilometraza
    };
    this.loadLaborAndMaterial(order.id);
    this.loadAllMaterials();
  }

  loadAllMaterials(): void {
    this.materialMasterService.getAllMaterials().subscribe({
      next: (materials) => this.allMaterials = materials,
      error: (err) => console.error('Error loading materials:', err)
    });
  }

  loadLaborAndMaterial(serviceOrderId: number): void {
    this.laborService.getLaborsByServiceOrderId(serviceOrderId).subscribe({
      next: (labors) => this.labors = labors,
      error: (err) => console.error('Error loading labors:', err)
    });
    
    this.materialService.getMaterialsByServiceOrderId(serviceOrderId).subscribe({
      next: (materials) => this.materials = materials,
      error: (err) => console.error('Error loading materials:', err)
    });
  }

  cancelEdit(): void {
    this.editingOrder = null;
    this.labors = [];
    this.materials = [];
  }

  updateOrder(): void {
    if (!this.updatedOrder.brojNaloga || !this.updatedOrder.datum || this.updatedOrder.clientId === 0 || 
        this.updatedOrder.vehicleId === 0 || !this.updatedOrder.opisRada) {
      alert('Popunite sva obavezna polja!');
      return;
    }

    this.serviceOrderService.updateServiceOrder(this.updatedOrder.id, this.updatedOrder).subscribe({
      next: (updated) => {
        const index = this.serviceOrders.findIndex(o => o.id === updated.id);
        if (index !== -1) {
          this.serviceOrders[index] = updated;
        }
        this.editingOrder = null;
      },
      error: (err) => {
        this.error = 'Greška prilikom ažuriranja servisnog naloga';
        console.error('Error updating service order:', err);
      }
    });
  }

  deleteOrder(id: number): void {
    if (!confirm('Da li ste sigurni da želite da obrišete ovaj servisni nalog?')) {
      return;
    }

    this.serviceOrderService.deleteServiceOrder(id).subscribe({
      next: () => {
        this.serviceOrders = this.serviceOrders.filter(o => o.id !== id);
      },
      error: (err) => {
        this.error = 'Greška prilikom brisanja servisnog naloga';
        console.error('Error deleting service order:', err);
      }
    });
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('sr-RS');
  }

  switchTab(tab: 'info' | 'labor' | 'material'): void {
    this.activeTab = tab;
  }

  // Labor functions
  addLabor(): void {
    if (!this.editingOrder) return;
    
    this.newLabor.serviceOrderId = this.editingOrder.id;
    
    if (!this.newLabor.opisRadova || this.newLabor.ukupnoVreme <= 0 || this.newLabor.cena <= 0) {
      alert('Popunite sva polja!');
      return;
    }

    this.laborService.createLabor(this.newLabor).subscribe({
      next: (labor) => {
        this.labors.push(labor);
        this.newLabor = {
          serviceOrderId: this.editingOrder!.id,
          opisRadova: '',
          ukupnoVreme: 0,
          cena: 0
        };
      },
      error: (err) => {
        this.error = 'Greška prilikom dodavanja rada';
        console.error('Error adding labor:', err);
      }
    });
  }

  deleteLabor(id: number): void {
    if (!confirm('Da li ste sigurni?')) return;

    this.laborService.deleteLabor(id).subscribe({
      next: () => {
        this.labors = this.labors.filter(l => l.id !== id);
      },
      error: (err) => {
        this.error = 'Greška prilikom brisanja rada';
        console.error('Error deleting labor:', err);
      }
    });
  }

  // Material functions
  onMaterialSelected(): void {
    if (this.newMaterial.materialId > 0) {
      const selectedMaterial = this.allMaterials.find(m => m.id === this.newMaterial.materialId);
      if (selectedMaterial) {
        this.newMaterial.jedinicnaCena = selectedMaterial.jedinicnaCena;
        this.calculateMaterialTotal();
      }
    }
  }

  calculateMaterialTotal(): void {
    this.newMaterial.ukupnaCena = this.newMaterial.kolicina * this.newMaterial.jedinicnaCena;
  }

  addMaterial(): void {
    if (!this.editingOrder) return;
    
    this.newMaterial.serviceOrderId = this.editingOrder.id;
    
    if (this.newMaterial.materialId <= 0 || this.newMaterial.kolicina <= 0) {
      alert('Izaberite materijal i unesite količinu!');
      return;
    }

    // Calculate total
    this.newMaterial.ukupnaCena = this.newMaterial.kolicina * this.newMaterial.jedinicnaCena;

    this.materialService.createMaterial(this.newMaterial).subscribe({
      next: (material) => {
        this.materials.push(material);
        this.newMaterial = {
          serviceOrderId: this.editingOrder!.id,
          materialId: 0,
          kolicina: 0,
          jedinicnaCena: 0,
          ukupnaCena: 0
        };
      },
      error: (err) => {
        this.error = 'Greška prilikom dodavanja materijala';
        console.error('Error adding material:', err);
      }
    });
  }

  deleteMaterial(id: number): void {
    if (!confirm('Da li ste sigurni?')) return;

    this.materialService.deleteMaterial(id).subscribe({
      next: () => {
        this.materials = this.materials.filter(m => m.id !== id);
      },
      error: (err) => {
        this.error = 'Greška prilikom brisanja materijala';
        console.error('Error deleting material:', err);
      }
    });
  }
}
