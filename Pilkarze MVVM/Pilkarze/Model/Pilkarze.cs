using System.Collections.ObjectModel;

namespace PilkarzeMVVM.Model
{
    class Pilkarze
    {
        public ObservableCollection<Pilkarz> PilkarzList { get; set; }

        public Pilkarze()
        {
            PilkarzList = new ObservableCollection<Pilkarz>();
        }

        public ObservableCollection<Pilkarz> DodajPilkarza(Pilkarz obj)
        {
            PilkarzList.Add(obj);
            return PilkarzList;
        }

        public ObservableCollection<Pilkarz> UsunPilkarza(Pilkarz obj)
        {
            PilkarzList.Remove(obj);
            return PilkarzList;
        }

        public ObservableCollection<Pilkarz> EdytujPilkarza(Pilkarz obj, Pilkarz newObj)
        {
            if (PilkarzList.Contains(obj))
            {
                int index = PilkarzList.IndexOf(obj);
                PilkarzList.Remove(obj);
                PilkarzList.Insert(index, newObj);
            }
            return PilkarzList;
        }
    }
}
