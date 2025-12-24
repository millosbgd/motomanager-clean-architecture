import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialService } from '../services/material.service';
import { Material, CreateMaterialRequest, UpdateMaterialRequest } from '../models/material.model';

@Component({
  selector: 'app-materials',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './materials.component.html',
  styleUrl: './materials.component.css'
})
export class MaterialsComponent implements OnInit {
  materials: Material[] = [];
  loading = false;
  error: string | null = null;
  
  // Pagination
  currentPage = 1;
  pageSize = 20;
  totalCount = 0;
  totalPages = 0;
  
  showAddForm = false;
  editingMaterial: Material | null = null;
  
  newMaterial: CreateMaterialRequest = {
    naziv: '',
    jedinicnaCena: 0
  };
  
  updatedMaterial: UpdateMaterialRequest = {
    id: 0,
    naziv: '',
    jedinicnaCena: 0
  };

  constructor(private materialService: MaterialService) { }

  ngOnInit(): void {
    this.loadMaterials();
  }

  loadMaterials(): void {
    this.loading = true;
    this.error = null;
    
    this.materialService.getMaterialsPaged(this.currentPage, this.pageSize).subscribe({
      next: (pagedResult) => {
        this.materials = pagedResult.items || [];
        this.totalCount = pagedResult.totalCount || 0;
        this.totalPages = pagedResult.totalPages || 0;
        this.currentPage = pagedResult.currentPage || 1;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška prilikom učitavanja materijala';
        this.loading = false;
        console.error('Error loading materials:', err);
      }
    });
  }

  showAddMaterialForm(): void {
    this.showAddForm = true;
    this.newMaterial = {
      naziv: '',
      jedinicnaCena: 0
    };
  }

  cancelAdd(): void {
    this.showAddForm = false;
  }

  addMaterial(): void {
    if (!this.newMaterial.naziv || this.newMaterial.jedinicnaCena <= 0) {
      alert('Popunite sva polja!');
      return;
    }

    this.materialService.createMaterial(this.newMaterial).subscribe({
      next: (material) => {
        this.materials.push(material);
        this.showAddForm = false;
      },
      error: (err) => {
        this.error = 'Greška prilikom dodavanja materijala';
        console.error('Error adding material:', err);
      }
    });
  }

  editMaterial(material: Material): void {
    this.editingMaterial = material;
    this.updatedMaterial = {
      id: material.id,
      naziv: material.naziv,
      jedinicnaCena: material.jedinicnaCena
    };
  }

  cancelEdit(): void {
    this.editingMaterial = null;
  }

  updateMaterial(): void {
    if (!this.updatedMaterial.naziv || this.updatedMaterial.jedinicnaCena <= 0) {
      alert('Popunite sva polja!');
      return;
    }

    this.materialService.updateMaterial(this.updatedMaterial.id, this.updatedMaterial).subscribe({
      next: (updated) => {
        const index = this.materials.findIndex(m => m.id === updated.id);
        if (index !== -1) {
          this.materials[index] = updated;
        }
        this.editingMaterial = null;
      },
      error: (err) => {
        this.error = 'Greška prilikom ažuriranja materijala';
        console.error('Error updating material:', err);
      }
    });
  }

  deleteMaterial(id: number): void {
    if (!confirm('Da li ste sigurni da želite da obrišete ovaj materijal?')) {
      return;
    }

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

  // Pagination methods
  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadMaterials();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadMaterials();
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadMaterials();
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
