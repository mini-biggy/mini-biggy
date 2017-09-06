namespace MiniBiggy {
    public static class Create {
        public static CreateListOf<T> ListOf<T>() where T : new() {
            return new CreateListOf<T>();
        }
    }
}