namespace DomainAnimal
{
    public struct AnimalConstants
    {
        public const string ANIMAL_CACHE_TAG = "animal:";
        public const string ANIMAL_COUNT_PREFIX = "animal:count";
        public const string ANIMAL_BY_ID_CACHE_TAG = "animal:id:";
        public const string ALL_ANIMALS_BY_ID_CACHE_TAG = "animal:id";
        public const string ANIMAL_OWNERLESS_CACHE_TAG = "animal:owerless_animals";
    }

    public struct EmployeeConstants
    {
        public const string EMPLOYEE_CACHE_TAG = "employee:";
        public const string EMPLOYEE_BY_ID_CACHE_TAG = "employee:id:";
        public const string EMPLOYEES_BY_POSITION_CACHE_TAG = "employee:employees_position:";
        public const string EMPLOYEES_WITHOUT_ANIMALS = "employee:employees_without_animals";
    }
}
