# Head Punch Demo

[preview png]

Данный проект разработан в рамках технического задания

Требования:
- Механика избиения головы
- Видимый урон
- Иммерсивность, креатив

## Что сделано

### 1. Механика ударов и оружие
В качестве сущности, которой можно наносить удары используется модель "манекена", которая состоит из:
* Головы, которая **сопротивляется ударам** и возвращается в исходное положение. Однако, с получением урона голова ослабевает и перестает так же активно сопротивляться ударам
* Торса, который при ослабевании просто наклоняется

При этом, урон с конечностей также передается в основу - сам манекен. При снижении здоровья манекена до нуля он распадается.

Если нанести **добивающий удар** по голове, она отлетит и включится отдельная камера с постобработкой, аналог **Kill Cam**

В проекте представлены 3 вида оружия:
1. Бита, наносящая удары сбоку
2. Автопанчер
3. Перчатки

<details open>
    <summary>Код реализации Joint</summary>
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Ragdoll/HeadJointController.cs#L1-L88
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Ragdoll/JointData.cs#L1-L27
    
</details>

<details open>
    <summary>Код реализации оружия</summary>
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Weapons/BaseSimpleWeapon.cs#L1-L63
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Weapons/Concrete/BatWeapon.cs#L1-L116
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Weapons/Concrete/GlovesWeapon.cs#L1-L59
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Weapons/Concrete/PuncherWeapon.cs#L1-L46
</details>

Ниже приведено видео, на котором показано нанесение урона оружием и соответствующее ослабевание головы при получении урона:

[head joint weakened mp4]

### 2. Механика покраски

Для нанесения видимого урона реализована механика покраски. Для этого используются шейдеры.

<details open>
    <summary>Код реализации</summary>
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Interactive/Painting/Paintable.cs#L1-L109
</details>

Основной шейдер выглядит следующим образом:

[shadergraph png]

Данный шейдер "наносит" текстурку краски поверх базовой текстуры объекта. Для улучшения визуала также строится карта нормалей по текстуре краски.

Нанесение краски и смешивание производится на GPU, с помощью шейдера-маски для нанесения цвета, шейдера-смешивания и Blit для копирования временной текстуры в текстуру краски.

Также реализована механика покраски предметов при столкновении.

Если красящим объектом выступает голова манекена, её цвет рассчитывается как среднее значение всех нанесённых по ней ударов. Например, два красных и два синих удара дадут фиолетовый результат при покраске.

<details open>
    <summary>Код реализации</summary>
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Interactive/Painting/CollisionPainter.cs#L1-L34
</details>

Ниже показан пример:

[head paintaing walls mp4]

### 3. Механика "смерти"

Манекен при достижении им нулевого значения здоровья распадается на составные части, которые представлены в коде следующими классами:

<details open>
    <summary>Код реализации</summary>
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Mannequin/Mannequin.cs#L1-L99
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Mannequin/ManneqBodyPart.cs#L1-L53
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Mannequin/ManneqHead.cs#L1-L17
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Mannequin/ManneqBody.cs#L1-L23
</details>

Видео-демонстрация данной механики:

[manneqs disassemble mp4]

### 4. Механика KillCam

Если добивающий удар наносится по голове, она отлетает от игрока, слегка поднимаясь вверх. В этот момент происходит переключение на вторую камеру, которая следует за ней до её исчезновения. К KillCam применены эффекты постобработки.

Для ожидания исчезновения головы используется CustomYieldInstruction:
<details open>
    <summary>Код реализации</summary>
    https://github.com/antonworkgit/HeadPunchDemo/blob/e995e4007ca3cb15d863554fa7dbdccea4791192/Assets/Scripts/Misc/WaitUntilDestroyed.cs#L1-L16
</details>

[kill cam mp4]

## Демонстрация "всего и сразу"

[all-in-one mp4]

## Использованные ассеты
Из сторонних ассетов использованы [Unity Starter Assets](https://assetstore.unity.com/packages/essentials/starter-assets-thirdperson-updates-in-new-charactercontroller-pa-196526?srsltid=AfmBOoonO0FBZKtXf811rI2cpkPu-NtOFVGmtaCapNzSipkZagdj_-Ve)