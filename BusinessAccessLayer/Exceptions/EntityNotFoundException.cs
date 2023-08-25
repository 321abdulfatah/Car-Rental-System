namespace BusinessAccessLayer.Exceptions
{
    public  class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base()
        {

        }
        public EntityNotFoundException(string massage) : base(massage) 
        {

        }
        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
