using Microsoft.AspNetCore.Http;
using SiteMovie.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SiteMovie.Domain.ViewModels
{
    public class ListMoviesViewModel
    {
        public int Id { get; set; }
        [Display(Name ="نام فیلم")]
        public string MovieTitle { get; set; }
        [Display(Name ="ستارگان")]
        public string Actors { get; set; }
        [Display(Name ="کشور سازنده")]
        public string Country { get; set; }
        [Display(Name ="کارگردان")]
        public string Director { get; set; }
        [Display(Name ="تاریخ انتشار")]
        public string PublishDate { get; set; }
        [Display(Name ="تصویر")]
        public string MovieImage { get; set; }
    }

    public class AddMovieViewModels
    {
        [Display(Name = "بازیگران")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Actors { get; set; }
        [Display(Name = "کشور سازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Country { get; set; }
        [Display(Name = "توضحیحات")]
        public string Description { get; set; }
        [Display(Name = "کارگردان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Director { get; set; }
        [Display(Name = "نام فیلم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string MovieTitle { get; set; }
        [Display(Name = "تصویر شاخص")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile MovieImage { get; set; }
        [Display(Name = "تاریخ انتشار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PublishDate { get; set; }
        [Display(Name = "نمره imdb")]
        public decimal imdbRating { get; set; }
        [Display(Name = "تیزر")]
        public IFormFile Demo { get; set; }
        public List<int> selectedGroups { get; set; }
    }

    public class EditMovieViewModels
    {
        public int Id { get; set; }
        [Display(Name = "بازیگران")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Actors { get; set; }
        [Display(Name = "کشور سازنده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Country { get; set; }
        public DateTime CreateDate { get; set; }
        [Display(Name = "توضحیحات")]
        public string Description { get; set; }
        [Display(Name = "کارگردان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Director { get; set; }
        [Display(Name = "نام فیلم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string MovieTitle { get; set; }
        [Display(Name = "تصویر شاخص")]
        public IFormFile MovieImage { get; set; }
        [Display(Name = "تاریخ انتشار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PublishDate { get; set; }
        [Display(Name = "نمره imdb")]
        public decimal imdbRating { get; set; }
        [Display(Name = "تیزر")]
        public IFormFile Demo { get; set; }
        public List<int> selectedGroups { get; set; }
    }

    public class ShowMovieViewModel
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string Description { get; set; }
        public string Actors { get; set; }
        public string Director { get; set; }
        public string PublishDate { get; set; }
        public decimal imdbRating { get; set; }
        // Users rating
        public decimal Rating { get; set; }
        public string Demo { get; set; }
        public List<string> Categories { get; set; }
    }

    public class ShowListMoviesViewModel
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string MovieImage { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Rate { get; set; }
    }

    public class SuggestedMoviesViewModel
    {
        public int MovieId { get; set; }
        public string MovieImage { get; set; }
        public string MovieTitle { get; set; }
        public decimal Rate { get; set; }
    }

    public class Pager
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public Pager()
        {

        }
        public Pager(int totalItems,int page,int pageSize=10)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if(startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
