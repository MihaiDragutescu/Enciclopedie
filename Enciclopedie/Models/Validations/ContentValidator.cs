using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
    
namespace Enciclopedie.Models.Validations
{
    public class ContentValidator: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var article = (Article)validationContext.ObjectInstance;
            string articleContent = article.Content;
            char[] charsArray = { '.', '!', '?' };
            int numberOfSentences = 0;
            bool cond = true;

            if (articleContent != null)
            {
                articleContent = string.Concat(articleContent.Where(c => !char.IsWhiteSpace(c)));

                if (articleContent.Length < 100 || articleContent.Length > 10000)
                {
                    cond = false;
                }

                if (cond)
                {
                    for (int i = 0; i < articleContent.Length; i++)
                    {
                        if (i == 0 && char.IsLetter(articleContent[i]))
                        {
                            if (!char.IsUpper(articleContent[i]))
                            {
                                cond = false;
                                break;
                            }
                        }
                        else
                        {
                            if (i != articleContent.Length - 1 && charsArray.Contains(articleContent[i]))
                            {
                                numberOfSentences += 1;

                                if (!char.IsUpper(articleContent[i + 1]) && char.IsLetter(articleContent[i + 1]))
                                {
                                    cond = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                cond = false;
            }

            if(numberOfSentences<3)
            {
                cond = false;
            }

            return cond ? ValidationResult.Success : new ValidationResult("Continutul articolului nu este valid!");
        }
    }
}